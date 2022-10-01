using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedMovingFeedBack : MonoBehaviour,ITurnDependant
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color darkColor;
    private Color originalColor;


    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void PlayFeedBack()
    {
        spriteRenderer.color = darkColor;
    }
    public void StopFeedBack()
    {
        spriteRenderer.color = originalColor;
    }

    public void WaitTurn()
    {
        StopFeedBack();
    }
}
