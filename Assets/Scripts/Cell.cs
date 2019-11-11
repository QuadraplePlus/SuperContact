using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICell
{
    void DidSelectCell(Cell cell);
    void DeleteCell(Cell cell);
}
public class Cell : MonoBehaviour
{
    [SerializeField] Text title;
    [SerializeField] Image titleImage;
    [SerializeField] public Button button;
    public ICell cellDelegate;
    [SerializeField] Button cellButton;
    public string Title
    {
        get
        {
            return this.title.text;
        }
        set
        {
            // title에 대한 유효성 체크
            this.title.text = value;
        }
    }
    public Sprite Sprite
    {
        get
        {
            return this.titleImage.sprite;
        }
        set
        {
            this.titleImage.sprite = value;
        }
    }
    public bool ActiveDelete
    {
        get
        {
            return button.gameObject.activeSelf;
        }
        set
        {
            button.gameObject.SetActive(value);

            if (value)
                cellButton.interactable = false;
            else
                cellButton.interactable = true;
        }
    }
    private void Start()
    {
        //button.gameObject.SetActive(false);
        
        this.ActiveDelete = false;
        cellButton.GetComponent<Button>();
    }
    public void OnClick()
    {
        cellDelegate.DidSelectCell(this);
    }
    public void OnClickDelete()
    {
        //팝업창 띄우기
        cellDelegate.DeleteCell(this);
    }
}
