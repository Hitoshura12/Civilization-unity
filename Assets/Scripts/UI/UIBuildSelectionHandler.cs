using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildSelectionHandler : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    private BuildDataSO buildData;

    private UIHighlight uiHighlight;
    private UIBuildButtonHandler buttonHandler;

    private CanvasGroup canvasGroup;
    [SerializeField]
    private bool interactable = false;

    public BuildDataSO BuildData { get => buildData;}

    private void Awake()
    {
       
        canvasGroup = GetComponent<CanvasGroup>();
        uiHighlight = GetComponent<UIHighlight>();
        buttonHandler = GetComponentInParent<UIBuildButtonHandler>();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable==false)
        {
            buttonHandler.ResetBuildButton();
            return;
        }
        buttonHandler.PreareBuildButton(BuildData);
        uiHighlight.ToggleHighlight(true);
        
       
    }
    public void Reset()
    {
        uiHighlight.ToggleHighlight(false);
    }

    public void ToggleActive(bool value)
    {
        interactable = value;
        canvasGroup.interactable = value;
        canvasGroup.alpha = value ? 1 : 0.5f;
    }
}
