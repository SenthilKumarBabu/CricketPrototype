using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    protected PanelManager panelManager;
    protected MatchInfo matchInfo;

    [SerializeField] protected bool showInStart;
    public abstract bool ShowInStart
    {
        get;
        set;
    }

    private void Awake()
    {
        panelManager = (PanelManager)FindObjectOfType(typeof(PanelManager));
        matchInfo = (MatchInfo)FindObjectOfType(typeof(MatchInfo));
        Register();
    }

    private void Start()
    {
        if (showInStart)
            ShowPanel();
        else
            HidePanel();
    }

    public void Register()
    {
        panelManager.panels.Add(new PanelInfo(this.gameObject, this));
    }

    public abstract void ShowPanel();

    public abstract void HidePanel();
}
