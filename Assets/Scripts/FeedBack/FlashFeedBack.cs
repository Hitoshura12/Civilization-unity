using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFeedBack : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private float invisibleTime, visibleTime;


    public void PlayFeedBack()
    {
        if (spriteRenderer == null)
            return;

        StopFeedBack();
        StartCoroutine(FlashCoroutine());
    }


    IEnumerator FlashCoroutine()
    {
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0;
        spriteRenderer.color = spriteColor;
        yield return new WaitForSeconds(invisibleTime);

        spriteColor.a = 1;
        spriteRenderer.color = spriteColor;
        yield return new WaitForSeconds(visibleTime);
        StartCoroutine(FlashCoroutine());
       
    }
    public void StopFeedBack()
    {
        StopAllCoroutines();
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 1;
        spriteRenderer.color = spriteColor;
    }
}
