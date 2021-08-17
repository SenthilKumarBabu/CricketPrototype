using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchInfo : MonoBehaviour
{
    [SerializeField] private PitchDimension[] pitchDimensions;
    [SerializeField] private int totalOvers;
    [SerializeField] private int currentScore;
    [SerializeField] private int ballsRemaining;
    [SerializeField] private int numOfMaxBatsman;
    [SerializeField] private int numOfBatsmanRemaining;
    [SerializeField] private int targetScore;
    [SerializeField] private int? selectedPitchIndex;
    [SerializeField] private Vector3 pitchingSpot;
    [SerializeField] private float wicketPercent;
    [SerializeField] private bool isGameOver;

    public const int NumOfBallsPerOver = 6;

    public int TotalOvers { get => totalOvers; set => totalOvers = value; }
    public int NumOfMaxBatsman { get => numOfMaxBatsman; set => numOfMaxBatsman = value; }
    public int TargetScore { get => targetScore; set => targetScore = value; }

    public Vector2Int GridSize => new Vector2Int(PitchDimensions[0].pitch.GetLength(0), PitchDimensions.GetLength(0));

    public PitchDimension[] PitchDimensions { get => pitchDimensions; set => pitchDimensions = value; }
   
    public int? SelectedPitchIndex { get => selectedPitchIndex; set => selectedPitchIndex = value; }
    public int CurrentScore { get => currentScore; set => currentScore = value; }
    public int BallsRemaining { get => ballsRemaining; set => ballsRemaining = value; }
    public int NumOfBatsmanRemaining { get => numOfBatsmanRemaining; set => numOfBatsmanRemaining = value; }
    public float WicketPercent { get => wicketPercent; set => wicketPercent = value; }
    public Vector3 PitchingSpot { get => pitchingSpot; set => pitchingSpot = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }

    private void OnEnable()
    {
        EventManager.onGameStart += GetInputs;
    }

    private void OnDisable()
    {
        EventManager.onGameStart -= GetInputs;
    }

    private void GetInputs()
    {
        TotalOvers = 5;
        NumOfMaxBatsman = 5;
        TargetScore = 60;
        BallsRemaining = TotalOvers * NumOfBallsPerOver;
        NumOfBatsmanRemaining = NumOfMaxBatsman;
        WicketPercent = 50;
    }

}

[System.Serializable]
public struct PitchDimension
{
    public PitchDetail[] pitch;
}

[System.Serializable]
public struct PitchDetail
{
    public string name;
    public float length;
    public float width;
}