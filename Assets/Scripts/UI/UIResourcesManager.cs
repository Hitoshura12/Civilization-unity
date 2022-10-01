using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResourcesManager : MonoBehaviour
{
    private Dictionary<ResourceType, UIRessource> resourcesDictionary 
        = new Dictionary<ResourceType, UIRessource>();

    private void Awake()
    {
        PrepareResourcesDictionary();
    }

    private void PrepareResourcesDictionary()
    {
        foreach (UIRessource resource in GetComponentsInChildren<UIRessource>())
        {
            if (resourcesDictionary.ContainsKey(resource.ResourceType))
            {
                throw new ArgumentException("Dictionary already contains a:" + resource.ResourceType.ToString());
            }
            resourcesDictionary[resource.ResourceType] = resource;
            SetResource(resource.ResourceType, 0);
        }
    }

    public void SetResource(ResourceType resourceType, int val)
    {
        try
        {
            resourcesDictionary[resourceType].SetValue(val);
        }
        catch (Exception)
        {

            throw new Exception("Dictionary doesn't contain UIResourceReference for:"+ resourceType);
        }
    }
}
