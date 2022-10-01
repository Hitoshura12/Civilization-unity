using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighlight : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Color highlightColor;
    private Color originalColor = Color.white;

    public void ToggleHighlight(bool val)
    {
        if (val)
        {
            image.color = highlightColor;
        }
        else
        {
            image.color = originalColor;
        }
    }

}
