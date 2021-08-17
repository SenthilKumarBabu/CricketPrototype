using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchHandler : MonoBehaviour
{
    private MatchInfo matchInfo;
    [SerializeField] private GameObject pitchPrefab;

    private void Awake()
    {
        matchInfo = (MatchInfo)FindObjectOfType(typeof(MatchInfo));
    }

    private void OnEnable()
    {
        //EventManager.onGameStart += CreateCustomPitch;
    }

    private void OnDisable()
    {
        //EventManager.onGameStart -= CreateCustomPitch;
    }

    public void CreateCustomPitch()
    {
        /*Debug.LogError(matchInfo.GridSize);
        float _TotalLength = 0, _CurrentLength = 0;
        float _TotalWidth = 0, _CurrentWidth = 0;

        for (int i = 0; i < matchInfo.GridSize.y; i++)
        {
            _TotalLength += matchInfo.PitchDimensions[i].pitch[0].length;
        }

        for (int i = 0; i < matchInfo.GridSize.x; i++)
        {
            _TotalWidth += matchInfo.PitchDimensions[0].pitch[i].length;
        }

        Debug.LogError($"{_TotalLength} {_TotalWidth}");

        for (int i = 0; i < matchInfo.GridSize.y; i++)
        {
            for (int j = 0; j < matchInfo.GridSize.x; j++)
            {
                float length = matchInfo.PitchDimensions[i].pitch[j].length;
                float width = matchInfo.PitchDimensions[i].pitch[j].width;
                _CurrentLength += length;
                GameObject _InsPitch = Instantiate(pitchPrefab,new Vector3(width, 0, _CurrentLength),Quaternion.identity, this.transform);
                _InsPitch.name = matchInfo.PitchDimensions[i].pitch[j].name;
                _InsPitch.transform.localScale = new Vector3(length, _InsPitch.transform.localScale.y, width);
            }
        }*/
    }


}
