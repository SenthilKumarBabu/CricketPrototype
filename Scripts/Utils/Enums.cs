[System.Serializable]
public enum BowlerType
{
    None,
    Fast,
    Spin
}

[System.Serializable]
public enum BallStatus
{
    None,
    Create,
    MovingTowardsPitchingSpot,
    MovingTowardsBatsman,
    MovingTowardsBoundary,
    CatchedByWicketKeeper,
    Bowled
}

[System.Serializable]
public enum BallOutput
{
    Hit,
    Out,
    Missed
}

[System.Serializable]
public enum UserInput
{
    Click,
    Drag
}

[System.Serializable]
public enum BowlingSide
{
    Left,
    Right
}
