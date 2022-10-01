using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    UIResourcesManager resourcesUI;

   private Dictionary<ResourceType, int> resourceDictionary
        = new Dictionary<ResourceType, int>();

    public List<ResourceValue> initialResources = new List<ResourceValue>();

    private void Start()
    {
        resourcesUI = FindObjectOfType<UIResourcesManager>();
        PrepareResourcesDictionary();
        SetInitialResourcesValues();
        UpdateUI();
    }

    private void SetInitialResourcesValues()
    {
        foreach (ResourceValue resourceValue in initialResources)
        {
            if (resourceValue.resourceType == ResourceType.None)
                throw new ArgumentException("Resources cannot be None!");
            resourceDictionary[resourceValue.resourceType] = resourceValue.resourceAmount;
        }
    }
    //Checks for the minimum allowed resource count for a specific resource in the resources dict.
    public bool CheckResourceAvailability(ResourceValue resourceRequired)
    {
        return resourceDictionary[resourceRequired.resourceType] >= resourceRequired.resourceAmount;

    }
    public void AddResources(List<ResourceValue> producedResources)
    {
        foreach (ResourceValue resourceVal in producedResources)
        {
            AddResource(resourceVal.resourceType, resourceVal.resourceAmount);
        }
    }
    private void AddResource(ResourceType resourceType, int resourceAmount)
    {
        resourceDictionary[resourceType] += resourceAmount;
        VerifyResourceAmount(resourceType);
        UpdateUI(resourceType);
    }
    public void SpendResource(List<ResourceValue> buildCost)
    {
        foreach (ResourceValue resourceValue in buildCost)
        {
            SpendResource(resourceValue.resourceType, resourceValue.resourceAmount);
        }
    }
    private void SpendResource(ResourceType resourceType, int resourceAmount)
    {
        resourceDictionary[resourceType] -= resourceAmount;
        VerifyResourceAmount(resourceType);
        UpdateUI(resourceType);
    }

    private void VerifyResourceAmount(ResourceType resourceType)
    {
        if (resourceDictionary[resourceType]<0)
        {
            throw new InvalidOperationException("Cannot have a negative resource!"+ resourceType);
        }
    }

    private void UpdateUI()
    {
        foreach (ResourceType resourceType in resourceDictionary.Keys)
        {
            UpdateUI(resourceType);
        }
    }

    private void UpdateUI(ResourceType resourceType)
    {
        resourcesUI.SetResource(resourceType, resourceDictionary[resourceType]);
    }

    private void PrepareResourcesDictionary()
    {
        foreach (ResourceType resourceValue in Enum.GetValues(typeof(ResourceType)))
        {
            if (resourceValue == ResourceType.None)
                continue;
            resourceDictionary[resourceValue] = 0;
        }
    }
}
[Serializable]
public struct ResourceValue
{
    public ResourceType resourceType;
    [Min(0)]
   public int resourceAmount;

}