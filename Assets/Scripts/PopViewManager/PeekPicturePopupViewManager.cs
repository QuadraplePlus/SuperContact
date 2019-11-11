using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PeekPicturePopupViewManager : PopupViewManager , IPhotoCell
{
    Sprite[] sprites;
    [SerializeField] GameObject photoCellPreFab;
    [SerializeField] RectTransform contecnt;
    [SerializeField] public ScrollRect scrollRect;
    [SerializeField] public GridLayoutGroup gridLayout;

    List<PhotoCell> PhotoCellList = new List<PhotoCell>();

    public static Action<Sprite> sendImage;
    float cellHeight;

    protected override void Awake()
    {
        base.Awake();
        sprites = SpriteManager.Load();
        AddPhotoCell();
    }
    public void AddPhotoCell()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            PhotoCell photoCell;
            photoCell = Instantiate(photoCellPreFab, contecnt).GetComponent<PhotoCell>();
            photoCell.GetComponent<Image>().sprite = sprites[i];
            photoCell.photoCellDelegater = this;
            PhotoCellList.Add(photoCell);
        }
        cellHeight = (gridLayout.cellSize.y + gridLayout.spacing.y)
           * (sprites.Length / gridLayout.constraintCount) + gridLayout.padding.top + gridLayout.padding.bottom
           + gridLayout.cellSize.y;

        scrollRect.content.sizeDelta = new Vector2(0, cellHeight);
    }
    public void ClosePopup()
    {
        PeekPictureClose();
    } 
    public void DidSelectedPhotoCell(PhotoCell photoCell)
    {
        PeekPictureClose();
        int index = PhotoCellList.IndexOf(photoCell);
        Debug.Log(index + "번 셀 선택");
        Sprite userImage = PhotoCellList[index].GetComponent<Image>().sprite;
        sendImage(userImage);
    }
}