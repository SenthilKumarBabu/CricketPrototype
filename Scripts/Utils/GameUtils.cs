using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static void RefreshTween(this Tweener tween)
    {
        if (tween.IsActive())
        {
            tween.Kill();
            tween = null;
        }
    }

    public static IEnumerator WaitForDelay(this IEnumerator enumerator, int delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public static IEnumerator WaitForDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public static int GetRandomExcludingList(int startValue, int endValue, List<int> list)
    {
        List<int> randomList = new List<int>();
        for (int i = 0, j = startValue; j < endValue; i++)
        {
            randomList.Add(j);
            j++;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if(randomList.Contains(list[i]))
            {
                randomList.Remove(list[i]);
            }
        }
        return randomList[UnityEngine.Random.Range(0,randomList.Count)];
    }
}
