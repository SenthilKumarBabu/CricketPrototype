using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceManager
{
    private static Dictionary<System.Type, MonoBehaviour> dict;

    public static T Get<T>()where T : MonoBehaviour
    {
        if (dict == null)
            dict = new Dictionary<System.Type, MonoBehaviour>();

        if(dict.ContainsKey(typeof (T)))
        {
            var temp = dict[typeof(T)];
            if(temp == null)
            {
                dict.Remove(typeof(T));
            }
            else
            {
                return temp as T;
            }
        }

        var value = Object.FindObjectOfType(typeof(T)) as MonoBehaviour;
        if(value != null)
        {
            dict[typeof(T)] = value;
            return value as T;
        }
        return null;
    }
}
