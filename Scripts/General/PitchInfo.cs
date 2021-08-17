using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchInfo : MonoBehaviour
{
    [SerializeField] private int index;
    public Transform PitchingSpot;

    public int Index { get => index; set => index = value; }
}
