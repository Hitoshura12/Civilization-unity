using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private Image infoImage;

    public void SetData(Sprite sprite, string text)
    {
        infoImage.sprite = sprite;
        nameText.text = text;
    }

    public void ToggleVisibility(bool val)
    {
        gameObject.SetActive(val);
    }
}
