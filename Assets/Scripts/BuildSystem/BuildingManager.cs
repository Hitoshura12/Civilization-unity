using UnityEngine;

public class BuildingManager : MonoBehaviour, ITurnDependant
{
    Unit unitFarmer;
    [SerializeField]
    private UIBuildButtonHandler unitBuildUI;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Map map;
    [SerializeField]
    private InfoManager infoManager;

    [SerializeField]
    private ResourcesManager resourcesManager;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void HandleSelection(GameObject selectedObject)
    {
        ResetBuildingSystem();
        if (selectedObject == null)
        {
            return;
        }
        Worker worker = selectedObject.GetComponent<Worker>();
        if (worker != null)
        {
            HandleUnitSelection(worker);
        }

    }

    private void HandleUnitSelection(Worker worker)
    {
        unitFarmer = worker.GetComponent<Unit>();
        if (unitFarmer != null && unitFarmer.CanStillMove())
        {     
            unitBuildUI.ToggleVisibility(true,resourcesManager);
            unitFarmer.FinishedMoving.AddListener(ResetBuildingSystem);
        }
    }

    private void ResetBuildingSystem()
    {
        if (unitFarmer != null)
        {
            unitFarmer.FinishedMoving.RemoveListener(ResetBuildingSystem);
        }
        unitFarmer = null;
        unitBuildUI.ToggleVisibility(false,resourcesManager);
    }

    public void BuildStructure(BuildDataSO buildData)
    {
        if (map.IsPositionInvalid(unitFarmer.transform.position))
        {
            Debug.LogWarning("Structure already exists here!");
            return;
        }
        Debug.Log($"Placing structure at{ unitFarmer.transform.position}");

        GameObject structure = Instantiate(buildData.prefab, unitFarmer.transform.position, Quaternion.identity);
        ResourceProducer resourceProducer = structure.GetComponent<ResourceProducer>();

        if (resourceProducer != null)
        {
            resourceProducer.Initialize(buildData);
        }


        map.AddStructure(unitFarmer.transform.position, structure);
        resourcesManager.SpendResource(buildData.buildCost);
        audioSource.Play();

        if (buildData.prefab.name == "TownStructure")
        {
            unitFarmer.DestroyUnit();
            infoManager.HideInfoPanel();
        }
        else
        {
            unitFarmer.FinishMovement();
        }
        ResetBuildingSystem();
    }
    public void WaitTurn()
    {
        ResetBuildingSystem();
    }
}
