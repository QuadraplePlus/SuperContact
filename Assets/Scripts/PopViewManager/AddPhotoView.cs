using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddPhotoView : PopupViewManager
{
    Sprite[] sprites;
    [SerializeField] GameObject photoCellPreFab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GridLayoutGroup gridLayout;

    List<PhotoCell> PhotoCellList = new List<PhotoCell>();

    public static Action<Sprite> sendImage;

    protected override void Awake()
    {
        base.Awake();
        photoCellPreFab.gameObject.SetActive(false);
        Sprite[] sprites = SpriteManager.Load();
        MakeImageCell(sprites);
    }

    private void MakeImageCell(Sprite[] sprites)
    {
        foreach (Sprite sprite in sprites)
        {
            ImageCell imageCell 
                = Instantiate(photoCellPreFab, scrollRect.transform).GetComponent<ImageCell>();

        }
    }


    public void ClosePopup()
    {
        PeekPictureClose();
    }


}
