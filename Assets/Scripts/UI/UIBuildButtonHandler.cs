using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UIBuildButtonHandler : MonoBehaviour
{
    [SerializeField]
    private Button buildButton;

    private BuildDataSO builData;

    [SerializeField]
    private Transform uiElementsTransforms;

    private List<UIBuildSelectionHandler> buildOptions;

    [SerializeField]
    private UnityEvent<BuildDataSO> OnBuildButtonClick;

    private void Start()
    {
        gameObject.SetActive(false);
        buildOptions = new List<UIBuildSelectionHandler>();
        foreach (Transform selectionHandler in uiElementsTransforms)
        {
            buildOptions.Add(selectionHandler.GetComponent<UIBuildSelectionHandler>());
        }
    }

   public void PreareBuildButton(BuildDataSO buildData)

    {
        ResetUIElements();
        this.builData = buildData;
        buildButton.gameObject.SetActive(true);
    }
    public void ResetBuildButton()
    {
        this.builData= null;
        buildButton.gameObject.SetActive(false);
        
    }
    public void HandleButtonClick()
    {
        OnBuildButtonClick?.Invoke(this.builData);
        ResetUIElements();
    }

    private void ResetUIElements()
    {
        foreach (UIBuildSelectionHandler item in buildOptions)
        {
            item.Reset();
        }
    }

    public void ToggleVisibility(bool val, ResourcesManager resourcesManager)
    {
        gameObject.SetActive(val);
        if (val==true)
        {
            PrepareBuildOptions(resourcesManager);
            ResetBuildButton();
            ResetUIElements();
        }
    }

    private void PrepareBuildOptions(ResourcesManager resourcesManager)
    {
        foreach (UIBuildSelectionHandler buildItem in buildOptions)
        {
            if (buildItem.BuildData== null)
            {
                buildItem.ToggleActive(false);
                continue;
            }
            buildItem.ToggleActive(true);
            foreach (ResourceValue resourceValue in buildItem.BuildData.buildCost)
            {
                if (resourcesManager.CheckResourceAvailability(resourceValue)==false)
                {
                    buildItem.ToggleActive(false);
                    break;
                }
            }
        }
    }
}
