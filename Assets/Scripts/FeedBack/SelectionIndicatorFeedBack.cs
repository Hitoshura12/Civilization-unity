using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicatorFeedBack : MonoBehaviour, ITurnDependant
{
    private int defaultSortingLayer;
    private int layerToUse;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        layerToUse = SortingLayer.NameToID("SelectedObject");
        defaultSortingLayer = spriteRenderer.sortingLayerID;
    }

    private void ToggleSelection (bool val)
    {
        if (val)
        {
            spriteRenderer.sortingLayerID = layerToUse;
        }
        else
        {
            spriteRenderer.sortingLayerID = defaultSortingLayer;
        }
    }

    public void Select()
    {
        ToggleSelection(true);
    }
    public void DeSelect()
    {
        ToggleSelection(false);
    }

    public void WaitTurn()
    {
        DeSelect();
    }
}
