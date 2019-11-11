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

    //스크롤 뷰 에 나타내어질 포토셀들의 인덱스의 지정용 리스트
    List<PhotoCell> PhotoCellList = new List<PhotoCell>();

    //스프라이트를 전달할 액션함수
    public static Action<Sprite> sendImage;
    //스크롤 뷰의 y축 길이
    float cellHeight;

    protected override void Awake()
    {
        base.Awake();
        sprites = SpriteManager.Load();
        AddPhotoCell();
    }
    //로드한 스프라이트를 토대로 추가된 포토셀을 설정하고 추가된 포토셀에 스프라이트를 할당
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
    //창닫기
    public void ClosePopup()
    {
        PeekPictureClose();
    } 
    //포토셀 선택 시 실행될 함수
    public void DidSelectedPhotoCell(PhotoCell photoCell)
    {
        PeekPictureClose();
        int index = PhotoCellList.IndexOf(photoCell);
        Debug.Log(index + "번 셀 선택");
        Sprite userImage = PhotoCellList[index].GetComponent<Image>().sprite;
        //액션함수로 선택된 인덱스의 포토셀의 스프라이트를 전달
        sendImage(userImage);
    }
}