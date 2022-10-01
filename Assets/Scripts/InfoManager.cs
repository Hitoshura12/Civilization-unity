using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour,ITurnDependant
{
    [SerializeField]
    private UIInfoPanel infoPanel;

    private void Start()
    {
        HideInfoPanel();
    }
    public void HandleSelection(GameObject selectedObject)
    {
        HideInfoPanel();
        if (selectedObject==null)
            return;
        InfoProvider provider = selectedObject.GetComponent<InfoProvider>();
        if (provider == null)
            return;
        ShowInfoPanel(provider);
        
    }
    private void ShowInfoPanel(InfoProvider provider)
    {
        infoPanel.ToggleVisibility(true);
        infoPanel.SetData(provider.Image, provider.NameToDisplay);
    }

    public void HideInfoPanel()
    {
        infoPanel.ToggleVisibility(false);
    }

    public void WaitTurn()
    {
        HideInfoPanel();
    }
}
