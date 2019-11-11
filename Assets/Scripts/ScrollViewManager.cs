using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrollViewManager : ViewManager, ICell , IComparer<Contact>
{
    [SerializeField] GameObject cellPrefab;  
    [SerializeField] GameObject addPopupViewPrefab;
    [SerializeField] GameObject detailViewPrefab;
    [SerializeField] GameObject cautionPanelPrefab;
    [SerializeField] RectTransform content;

    //Cell 인덱스 관리용 리스트
    List<Cell> cellList = new List<Cell>();

    //기본 스프라이트
    Sprite defaultSprite;

    public static Action<bool> activeAction;

    // 셀 삭제버튼이 눌러졌을때 전달할 델리게이트
    public delegate void DeletedSelct();
    public DeletedSelct deletedSelct;
    //셀의 버튼 온 오프 관련 변수
    bool isSelectCell = false;

    //셀의 y축 길이
    float cellHeight = 330f;
    //편집모드 온 오프 토글 판단여부
    bool deleteMode = false;
    
    Contacts? contacts;
    private void Awake() 
    {
        // Title 지정
        title = "수퍼연락처";
        isSelectCell = true;

        //편집버튼 생성 및 토글 
        leftNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        leftNavgationViewButton.SetTitle((deleteMode) ? "완료" : "편집");
        leftNavgationViewButton.SetOnClickAction(() =>
        {
            deleteMode = !deleteMode;

            if (deleteMode)
            {
                isSelectCell = false;
                rightNavgationViewButton.GetComponent<Button>().interactable = false;
                leftNavgationViewButton.SetTitle("완료");
                foreach (Cell cell in cellList)
                {
                    cell.ActiveDelete = true;                
                }
            }
            else
            {
                isSelectCell = true;
                rightNavgationViewButton.GetComponent<Button>().interactable = true;
                leftNavgationViewButton.SetTitle("편집");
                foreach (Cell cell in cellList)
                {
                    cell.ActiveDelete = false;
                }
            }
        });     
        // Add 버튼 지정
        rightNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        rightNavgationViewButton.SetTitle("추가");
        rightNavgationViewButton.SetOnClickAction(() =>
        {
            // AddPopupViewManager를 표시하는 동작 구현
            AddPopupViewManager addPopupViewManager =
                Instantiate(addPopupViewPrefab, mainManager.transform).GetComponent<AddPopupViewManager>();

            // 새로운 연락처를 추가했을때 할 일
            addPopupViewManager.addContactCallback = (contact) =>
            {
                AddContact(contact);
                ClearCell();
                LoadData();
            };
            // AddPopupViewManager 열기
            addPopupViewManager.Open();
        });
        //디폴트 이미지 지정
        defaultSprite = Resources.Load<Sprite>(Constant.kDefaultUserImage);
    }
    private void Start() 
    {
        contacts = FileManager<Contacts>.Load(Constant.kFileName);
        LoadData();
    }   
    // TODO: Contacts에 있는 정보를 Cell로 만들어서 추가
    void LoadData()
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;
            contactsValue.contactList.Sort((x, y) =>
            {
                return Compare(x, y);
            });
            for (int i = 0; i < contactsValue.contactList.Count; i++)
            {
                AddCell(contactsValue.contactList[i], i);
            }
        }
    }
    // Contact 정보로 Cell 객체를 만들어서 content에 추가하는 함수
    void AddCell(Contact contact, int index)
    {
        Cell cell;
        cell = Instantiate(cellPrefab, content).GetComponent<Cell>();
        cell.Title = contact.name;

        cell.Sprite = SpriteManager.GetSprite(contact.photoName);
        if (cell.Sprite == null)
        {
            cell.Sprite = defaultSprite;
        }

        cell.cellDelegate = this;
        cellList.Add(cell);

        // Content의 높이 재조정
        AdjustContent();
    }
    // Contacts에 Contact 추가
    void AddContact(Contact contact)
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;
            contactsValue.contactList.Add(contact);
        }
        else
        {
            List<Contact> contactsList = new List<Contact>();
            contactsList.Add(contact);

            contacts = new Contacts(contactsList);
        }
    }
    // Content의 높이 재조정
    void AdjustContent()
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;
            content.sizeDelta = new Vector2(0, contactsValue.contactList.Count * cellHeight);
        }
        else
        {
            content.sizeDelta = Vector2.zero;
        }
    }
    //앱 종료시 저장
    void OnApplicationQuit()
    {
        if (contacts.HasValue)
            FileManager<Contacts>.Save(contacts.Value, Constant.kFileName);
    }
    // Cell이 터치 되었을때 호출하는 함수
    public void DidSelectCell(Cell cell)
    {    
        // TODO: 어떤 Cell이 선택되었는지 확인 후
        // Cell과 관련된 Detail 화면 표시
        if (contacts.HasValue && isSelectCell == true) 
        {      
            leftNavgationViewButton.gameObject.SetActive(false);
            int cellIndex = cellList.IndexOf(cell);
            DetatilViewManager detailViewManager
                = Instantiate(detailViewPrefab).GetComponent<DetatilViewManager>();
            
            Contact selectedContact = contacts.Value.contactList[cellIndex];
            detailViewManager.contact = selectedContact;

            detailViewManager.userImage.sprite = cell.Sprite;

            if (detailViewManager.userImage.sprite == null)
            {
                detailViewManager.userImage.sprite = defaultSprite;
            }
            
            detailViewManager.saveDelegate = (newContact) =>
            {
                contacts.Value.contactList[cellIndex] = newContact;
                ClearCell();
                LoadData();
            };
            mainManager.PresentViewManager(detailViewManager);     
        } 
    }
    //Cell 리스트에 있는 모든 셀을 삭제
    public void ClearCell()
    {
        foreach (Cell cell in cellList)
        {
            Destroy(cell.gameObject);
        }
        cellList.RemoveRange(0, cellList.Count);
    }
    //셀 삭제 로직
    public void DeleteCell(Cell cell)
    {
        CormfirmPopupViewManager cautionManager
            = Instantiate(cautionPanelPrefab, mainManager.transform).GetComponent<CormfirmPopupViewManager>();
        cautionManager.gameObject.SetActive(true);

        Debug.Log("델리게이트~");
        cautionManager.Open();

        cautionManager.deleteAction = () =>
        {
            //Cell 오브젝트 삭제 , 
            //contacts 있는 contactList 에서 contact 정보를 삭제 
            if (contacts.HasValue)
            {
                int cellIndex = cellList.IndexOf(cell);
                List<Contact> contactList = contacts.Value.contactList;
                contactList.RemoveAt(cellIndex);
                cellList.RemoveAt(cellIndex);
                Destroy(cell.gameObject);
                AdjustContent();
                Debug.Log(cellIndex + "번 인덱스 삭제");
            }
        };
    }
    public int Compare(Contact x, Contact y)
    {
        return x.name.CompareTo(y.name);
    }
}