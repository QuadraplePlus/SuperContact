using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] GameObject addPhotoPopupViewPrefab;
    Button cachedButton;

    Button CacheButton
    {
        get
        {
            if (cachedButton == null)
            {
                cachedButton = GetComponent<Button>();
            }
            return cachedButton;
        }
    }

    public Sprite Image
    {
        get
        {
            return this.buttonImage.sprite;
        }
        set
        {
            buttonImage.sprite = value;
        }
    
    }

    public bool Editable
    {
        get
        {
            return this.CacheButton.interactable;
        }
        set
        {
            this.CacheButton.interactable = value;
        }
    }

    public void Onclick()
    {
        if(addPhotoPopupViewPrefab)
        {
            PeekPicturePopupViewManager peekPicturePopupViewManager
                = Instantiate(addPhotoPopupViewPrefab, MainManager.Instance.transform).GetComponent<PeekPicturePopupViewManager>();

            peekPicturePopupViewManager.PeekPictureOpne();
            PeekPicturePopupViewManager.sendImage = (sprite) =>
            {
                this.Image = sprite;
            };
        }
    }
}
