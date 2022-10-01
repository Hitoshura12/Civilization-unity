using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationManager : MonoBehaviour,ITurnDependant
{
    [SerializeField]
    private UIBuildButtonHandler townUi;

    private Town selectedTown =null;

    [SerializeField]
    private ResourcesManager resourcesManager;

    public void HandleSelection(GameObject selectedObject)
    {
        ResetTownBuildUI();
        if (selectedObject == null)
            return;

         selectedTown = selectedObject.GetComponent<Town>();
        if (selectedTown != null)
        {
            HandleTown(selectedTown);
        }
    }

    private void HandleTown(Town selectedTown)
    {
        townUi.ToggleVisibility(true,resourcesManager);

    }
    public void CreateUnit(BuildDataSO buildData)
    {
        if (selectedTown.InProduction)
        {
            Debug.Log("This town is already producing a unit!");
            return;
        }
        resourcesManager.SpendResource(buildData.buildCost);
        selectedTown.AddUnitToProduction(buildData.prefab);
        ResetTownBuildUI();
    }
    //public void CreateUnit(GameObject unitToCreate)
    //{
    //    if (selectedTown.InProduction)
    //    {
    //        Debug.Log("This town is already producing a unit!");
    //        return;
    //    }
    //    selectedTown.AddUnitToProduction(unitToCreate);
    //    ResetTownBuildUI();
    //}
    private void ResetTownBuildUI()
    {
        townUi.ToggleVisibility(false,resourcesManager);
        selectedTown = null;
    }

    public void WaitTurn()
    {
        ResetTownBuildUI();
    }
}
