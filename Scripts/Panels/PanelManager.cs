using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public List<PanelInfo> panels;
}

[System.Serializable]
public class PanelInfo
{
    public GameObject objRef;
    public Panel scriptRef;

    public PanelInfo(GameObject Go, Panel script)
    {
        objRef = Go;
        scriptRef = script;
    }
}
