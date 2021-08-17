using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    private BallStatus currentStatus;

    public BallStatus CurrentStatus { get => currentStatus; set => currentStatus = value; }
}
