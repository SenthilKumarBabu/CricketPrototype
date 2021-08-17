using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlerScoreCard : Panel
{
    public override bool ShowInStart
    {
        get
        {
            return ShowInStart;
        }
        set
        {
            ShowInStart = value;
        }
    }

    private bool isBowlerTypeUpdated;
    public bool IsBowlerTypeUpdated { get => isBowlerTypeUpdated; set => isBowlerTypeUpdated = value; }


    public override void ShowPanel()
    {
        this.gameObject.SetActive(true);
    }

    public override void HidePanel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnFastButtonClicked()
    {
        EventManager.CurrentBowlerType = BowlerType.Fast;
        IsBowlerTypeUpdated = true;
        HidePanel();
        int index = panelManager.panels.FindIndex(d => d.scriptRef is InGamePanel);
        if(index >= 0)
        {
            panelManager.panels[index].scriptRef.ShowPanel();
        }
    }

    public void OnSpinButtonClicked()
    {
        EventManager.CurrentBowlerType = BowlerType.Spin;
        IsBowlerTypeUpdated = true;
        HidePanel();
        int index = panelManager.panels.FindIndex(d => d.scriptRef is InGamePanel);
        if (index >= 0)
        {
            panelManager.panels[index].scriptRef.ShowPanel();
        }
    }
}

