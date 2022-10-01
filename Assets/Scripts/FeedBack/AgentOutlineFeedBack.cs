using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentOutlineFeedBack : MonoBehaviour
{
    [SerializeField]
    private Renderer outlineRenderer;
    private Material agentMaterial;

    [SerializeField]
    private bool toggleOutline = true, changeColor = false;
    
    [SerializeField]
    Color colorToChange;
    Color originalColor;

    private void Start()
    {
        agentMaterial = outlineRenderer.material;
        originalColor = agentMaterial.GetColor("_Color");
       
    }

    private void ApplyChanges(bool value)
    {
        if (toggleOutline)
        {
            agentMaterial.SetInt("_Outline", value? 1 : 0);
        }
        if (changeColor)
        {
            agentMaterial.SetColor("_Color", value? colorToChange: originalColor);
        }
    }

    public void Select()
    {
        ApplyChanges(true);
    }
    public void Deselect()
    {
        ApplyChanges(false);
    }
}
