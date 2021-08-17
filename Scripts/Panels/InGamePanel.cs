using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InGamePanel : Panel
{
    public override bool ShowInStart
    {
        get
        {
            return ShowInStart;
        }
        set
        {
            ShowInStart = value;
        }
    }
    private BallInfo ballMovements;

    [SerializeField] private float responseDuration;
    [SerializeField] private TMP_Text deliveryTypeText, resultText, currentScoreText, ballsRemainingText,targetScoreText,remainingBatsmanText,matchStatusText;
    [SerializeField] private GameObject possibleRunsParent;

    [SerializeField] private Dictionary<int, float> PossibleRuns = new Dictionary<int, float>()
    {
        {0, 90},
        {1, 85},
        {2, 60},
        {4, 35},
        {6, 20}
    };

    private void SetDeliveryText()
    {
        if(matchInfo.SelectedPitchIndex == null)
        {
            deliveryTypeText.text = $"Delivery Type : None";
        }
        else
        {
            deliveryTypeText.text = $"Delivery Type : {EventManager.CurrentBowlerType} ball {matchInfo.SelectedPitchIndex}";
        }
    }

    private void SetCurrentScoreText()
    {
        currentScoreText.text = $"Current Score : {matchInfo.CurrentScore}";
    }

    private void SetBallsRemainingText()
    {
        ballsRemainingText.text = $"Remaining Balls : {matchInfo.BallsRemaining}";
    }

    private void SetRemainingBatsmanText()
    {
        remainingBatsmanText.text = $"Remaining Batsman : {matchInfo.NumOfBatsmanRemaining}";
    }

    private void SetTargetScoreText()
    {
        targetScoreText.text = $"Target Score : {matchInfo.TargetScore}";
    }

    private void SetMatchStatusText()
    {
        if (matchInfo.CurrentScore >= matchInfo.TargetScore)
        {
            matchStatusText.text = "Batsman Win!";
            matchInfo.IsGameOver = true;
        }
        else if (matchInfo.NumOfBatsmanRemaining <= 0 || matchInfo.BallsRemaining <= 0)
        {
            matchStatusText.text = "Bowlers Win!";
            matchInfo.IsGameOver = true;
        }
        else
        {
            matchStatusText.text = "";
            matchInfo.IsGameOver = false;
        }
    }

    private void OnEnable()
    {
        EventManager.onGameStart += SetCurrentScoreText;
        EventManager.onGameStart += SetBallsRemainingText;
        EventManager.onGameStart += SetRemainingBatsmanText;
        EventManager.onGameStart += SetTargetScoreText;
        EventManager.onPitchClicked += AfterPitchSelected;


        ballMovements = (BallInfo)FindObjectOfType(typeof(BallInfo));
    }

    private void OnDisable()
    {
        EventManager.onGameStart -= SetCurrentScoreText;
        EventManager.onGameStart -= SetBallsRemainingText;
        EventManager.onGameStart -= SetRemainingBatsmanText;
        EventManager.onGameStart -= SetTargetScoreText;
        EventManager.onPitchClicked -= AfterPitchSelected;
    }

    public override void ShowPanel()
    {
        this.gameObject.SetActive(true);
    }

    public override void HidePanel()
    {
        this.gameObject.SetActive(false);
    }

    private void AfterPitchSelected()
    {
        possibleRunsParent.SetActive(true);
        SetDeliveryText();
    }

    public void RunButtonClicked(int keyvalue)
    {
        EventManager.canCreateBall = true;
        float randomValue = Random.Range(0, 100);

        if (randomValue < PossibleRuns[keyvalue])
        {
            EventManager.CurrentBallOutput = BallOutput.Hit;
            resultText.text = $"Result : Hit";
            matchInfo.CurrentScore += keyvalue;
        }
        else
        {
            int missedValue = 100 - (int)PossibleRuns[keyvalue];
            int wicketValue = (int)(missedValue * (matchInfo.WicketPercent / 100));
            int randomWicketValue = Random.Range(0, missedValue); 
            if (randomWicketValue < wicketValue)
            {
                EventManager.CurrentBallOutput = BallOutput.Out;
                resultText.text = $"Result : Out";
                matchInfo.NumOfBatsmanRemaining--;
                SetRemainingBatsmanText();
            }
            else
            {
                EventManager.CurrentBallOutput = BallOutput.Missed;
                resultText.text = $"Result : Missed";
            }
        }
        matchInfo.BallsRemaining--;

        StartCoroutine(GameUtils.WaitForDelay(responseDuration * 0.5f, () => resultText.DOFade(0, responseDuration * 0.25f)));
        StartCoroutine(GameUtils.WaitForDelay(responseDuration * 0.75f, () => { resultText.text = $"Result : None"; resultText.DOFade(1, responseDuration * 0.25f); }));

        possibleRunsParent.SetActive(false);
        matchInfo.SelectedPitchIndex = null;
        SetDeliveryText();
        SetCurrentScoreText();
        SetBallsRemainingText();
        SetMatchStatusText();
    }
}


