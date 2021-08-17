using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfo : MonoBehaviour
{
    public GameObject BallPrefab;
    [SerializeField] private Vector3 leftArmPosition,rightArmPosition;
    public float Speed;
    [SerializeField] private float FastSpeed, SpinSpeed;

    public Ball CreateBall()
    {
        GameObject GO = Instantiate(BallPrefab, (EventManager.CurrentBowlingSide == BowlingSide.Left)?leftArmPosition : rightArmPosition, Quaternion.identity);
        if(EventManager.CurrentBowlerType == BowlerType.Fast)
        {
            Speed = FastSpeed;
        }
        else if(EventManager.CurrentBowlerType == BowlerType.Spin)
        {
            Speed = SpinSpeed;
        }
        GO.GetComponent<Ball>().CurrentStatus = BallStatus.Create;
        return GO.GetComponent<Ball>();
    }
}
