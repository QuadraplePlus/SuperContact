using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ImageCell : MonoBehaviour
{
    Image image;

    public Action<Sprite> onClickAction;

    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            {
                onClickAction(this.image.sprite);
            }
        });
    }
    public void SetImageCell(Sprite sprite, Action<Sprite> onClickAction)
    {
        this.image.sprite = sprite;
        this.onClickAction = onClickAction;
    }
}
