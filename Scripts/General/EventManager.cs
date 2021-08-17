using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private PanelManager panelManager;
    private BowlerScoreCard bowlerScoreCard;
    private MatchInfo matchInfo;
    private BallInfo ballInfo;
    private Ball currentBall;

    public static UserInput CurrentUserInput = UserInput.Drag;
    public static BowlingSide CurrentBowlingSide = BowlingSide.Left;
    public static BowlerType CurrentBowlerType = BowlerType.None;
    public static BallOutput CurrentBallOutput = BallOutput.Hit;
    public static bool canCreateBall;

    public delegate void OnGameStart();
    public static event OnGameStart onGameStart;

    public delegate void OnPitchClicked();
    public static event OnPitchClicked onPitchClicked;

    private void Awake()
    {
        matchInfo = (MatchInfo)FindObjectOfType(typeof(MatchInfo));
        ballInfo = (BallInfo)FindObjectOfType(typeof(BallInfo));
        panelManager = (PanelManager)FindObjectOfType(typeof(PanelManager));
        bowlerScoreCard = (BowlerScoreCard)FindObjectOfType(typeof(BowlerScoreCard));
    }

    public void Start()
    {
        onGameStart?.Invoke();
    }

    public void Update()
    {
        if (matchInfo.IsGameOver)
            return;

        if(CheckForOverComplete() && bowlerScoreCard.IsBowlerTypeUpdated == false && currentBall == null && canCreateBall == false)
        {
            ShowBowlingScoreCardAfterOverComplete();
            return;
        }

        if(CheckForOverComplete() == false)
            bowlerScoreCard.IsBowlerTypeUpdated = false;

        if (CurrentUserInput == UserInput.Click)
            ClickPitchingSpot();
        else if(CurrentUserInput == UserInput.Drag)
            DragPitchingSpot();
        CreateBall();
        DeliverBallToPitchingSpot();
        MoveTowardsBatsman();
        BallHittedToBoundary();
        BallCatchedByWitcketKeeper();
        BallTookWitcket();
    }

    private bool CheckForOverComplete() => (matchInfo.BallsRemaining % 6 == 0);

    private void ShowBowlingScoreCardAfterOverComplete()
    {
        if (bowlerScoreCard.isActiveAndEnabled)
            return;

        CurrentBowlerType = BowlerType.None;
        int prevIndex = panelManager.panels.FindIndex(d => d.scriptRef is InGamePanel);

        if (prevIndex >= 0)
        {
            panelManager.panels[prevIndex].scriptRef.HidePanel();
        }

        int nextIndex = panelManager.panels.FindIndex(d => d.scriptRef is BowlerScoreCard);
        if (nextIndex >= 0)
        {
            panelManager.panels[nextIndex].scriptRef.ShowPanel();
        }
    }

    private void ClickPitchingSpot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.GetComponent<PitchInfo>() != null)
                {
                    if (matchInfo.SelectedPitchIndex == null && currentBall == null)
                    {
                        matchInfo.SelectedPitchIndex = hit.transform.GetComponent<PitchInfo>().Index;
                        matchInfo.PitchingSpot = hit.transform.GetComponent<PitchInfo>().PitchingSpot.transform.position;
                        onPitchClicked?.Invoke();
                    }
                }
            }
        }
    }

    private void DragPitchingSpot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.GetComponent<PitchInfo>() != null)
                {
                    if (currentBall == null)
                    {
                        matchInfo.SelectedPitchIndex = hit.transform.GetComponent<PitchInfo>().Index;
                        hit.transform.GetComponent<PitchInfo>().PitchingSpot.transform.position = hit.point;
                        matchInfo.PitchingSpot = hit.transform.GetComponent<PitchInfo>().PitchingSpot.transform.position;
                        onPitchClicked?.Invoke();
                    }
                }
            }
        }
    }

    private void CreateBall()
    {
        if (canCreateBall == false)
            return;

        currentBall = ballInfo.CreateBall();
        currentBall.transform.LookAt(matchInfo.PitchingSpot);
        currentBall.CurrentStatus = BallStatus.MovingTowardsPitchingSpot;
        canCreateBall = false;
    }

    private void DeliverBallToPitchingSpot()
    {
        if (currentBall == null)
            return;

        if (currentBall.CurrentStatus != BallStatus.MovingTowardsPitchingSpot)
            return;

        float step = ballInfo.Speed * Time.deltaTime;
        currentBall.transform.position = Vector3.MoveTowards(currentBall.transform.position, matchInfo.PitchingSpot, step);

        if (Vector3.Distance(currentBall.transform.position, matchInfo.PitchingSpot) < 0.001f)
        {
            currentBall.transform.position = matchInfo.PitchingSpot;
            currentBall.transform.localEulerAngles = new Vector3(-currentBall.transform.localEulerAngles.x, currentBall.transform.localEulerAngles.y, currentBall.transform.localEulerAngles.z);
            currentBall.CurrentStatus = BallStatus.MovingTowardsBatsman;
        }
    }

    private void MoveTowardsBatsman()
    {
        if (currentBall == null)
            return;

        if (currentBall.CurrentStatus != BallStatus.MovingTowardsBatsman)
            return;

        currentBall.transform.Translate(Vector3.forward * Time.deltaTime * ballInfo.Speed);

        if(currentBall.transform.localPosition.z >= 6)
        {
            if(CurrentBallOutput == BallOutput.Hit)
            {
                currentBall.transform.localEulerAngles = new Vector3(Random.Range(315, 360), Random.Range(90, 270), 0);
                currentBall.CurrentStatus = BallStatus.MovingTowardsBoundary;
                StartCoroutine(GameUtils.WaitForDelay(1f, () =>
                {
                    if (currentBall != null)
                    {
                        Destroy(currentBall.gameObject);
                        currentBall = null;
                    }
                }));
            }
            else if(CurrentBallOutput == BallOutput.Missed)
            {
                currentBall.CurrentStatus = BallStatus.CatchedByWicketKeeper;
            }
            else if(CurrentBallOutput == BallOutput.Out)
            {
                currentBall.CurrentStatus = BallStatus.Bowled;
            }
        }
    }

    private void BallHittedToBoundary()
    {
        if (currentBall == null)
            return;

        if (currentBall.CurrentStatus != BallStatus.MovingTowardsBoundary)
            return;

        currentBall.transform.Translate(Vector3.forward * Time.deltaTime * ballInfo.Speed);
    }

    private void BallCatchedByWitcketKeeper()
    {
        if (currentBall == null)
            return;

        if (currentBall.CurrentStatus != BallStatus.CatchedByWicketKeeper)
            return;

        currentBall.transform.Translate(Vector3.forward * Time.deltaTime * ballInfo.Speed);

        if (currentBall.transform.localPosition.z >= 7)
        {
            currentBall.CurrentStatus = BallStatus.None;
            Destroy(currentBall.gameObject, 1f);
            currentBall = null;
        }
    }

    private void BallTookWitcket()
    {
        if (currentBall == null)
            return;

        if (currentBall.CurrentStatus != BallStatus.Bowled)
            return;

        currentBall.CurrentStatus = BallStatus.None;
        Destroy(currentBall.gameObject, 1f);
        currentBall = null;
    }
}
