using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProducer : MonoBehaviour,ITurnDependant
{
   private ResourcesManager resourcesManager;
   private BuildDataSO buildData;

    private void Awake()
    {
        if (resourcesManager==null)
        {
            resourcesManager = FindObjectOfType<ResourcesManager>();
        }
    }

    public void Initialize(BuildDataSO buildData) 
    {
        this.buildData = buildData;
    }

    public void WaitTurn()
    {
        resourcesManager.AddResources(buildData.producedGoods);
    }
}
