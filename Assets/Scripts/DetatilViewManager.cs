using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

//인풋필드의 확장 클래스
public static class SCInptuField
{
    //온 오프 시 글자색 및 인풋필드 활성화 비활성화
    public static void SetImnutable(this InputField inputField ,bool editMode)
    {
        Debug.Log("!!");
        if (editMode)
        {
            inputField.transform.Find("Text").GetComponent<Text>().color = Color.black;
        }
        else
        {
            inputField.transform.Find("Text").GetComponent<Text>().color = Color.white;

        }
        inputField.transform.Find("Placeholder").gameObject.SetActive(editMode);
        inputField.GetComponent<InputField>().interactable = editMode;
        inputField.GetComponent<Image>().enabled = editMode;
    }
}

public class DetatilViewManager : ViewManager
{
    [SerializeField] public InputField nameInputField;
    [SerializeField] public InputField phonNumInputField;
    [SerializeField] public InputField emailInputField;
    [SerializeField] public Image userImage;
    [SerializeField] Button saveButton;
    [SerializeField] Button imageButton;
    [SerializeField] GameObject PeekPopupViewPrefab;
    [HideInInspector] public Contact? contact;

    //디테일 뷰에서 수정후 저장 했을 시 정보를 전달할 델리게이트함수
    public delegate void DetailViewManagerSaveDelegate(Contact contact);
    public DetailViewManagerSaveDelegate saveDelegate;

    EventSystem system;
    
    //에디트모드 온 오프 확인여부
    bool editMode = true;

 
    private void Awake()
    {
        // Title 지정
        title = "상세화면";

        // Add 버튼 지정
        rightNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        rightNavgationViewButton.SetTitle("편집");
        rightNavgationViewButton.SetOnClickAction(() =>
        {
            ToggleSetEditMode();
        });

        //PeekPicturePopupViewManager 에서 액션함수로 전달받은 스프라이트 정보
        PeekPicturePopupViewManager.sendImage = (sprite) =>
        {
            userImage.sprite = sprite;
        };
    }
    //에디트 모드 토글
    void ToggleSetEditMode(bool updateInputField = false)
    {
        editMode = !editMode;

        saveButton.gameObject.SetActive(editMode);
        imageButton.enabled = editMode;

        nameInputField.SetImnutable(editMode);
        phonNumInputField.SetImnutable(editMode);
        emailInputField.SetImnutable(editMode);

        if (editMode)
        {
            rightNavgationViewButton.SetTitle("취소");
            Debug.Log("에디트 모드");    
        }
        else
        {
            rightNavgationViewButton.SetTitle("편집");
            if (contact.HasValue && !updateInputField)
            {
                Contact contactVal = contact.Value;
                nameInputField.text = contactVal.name;
                phonNumInputField.text = contactVal.phoneNumber;
                emailInputField.text = contactVal.email;
            }
            Debug.Log("에디트 모드 아님 ");
        }
    }
    private new void Start()
    {
        ToggleSetEditMode();
        system = EventSystem.current;
    }
    //디테일뷰 삭제 시 오른쪽 버튼 삭제
    private void OnDestroy()
    {
        Destroy(rightNavgationViewButton.gameObject);
    }
    //디테일 뷰에서 편집 후 세이브
    public void Save()
    {
        string name = nameInputField.text;
        string phoneNum = phonNumInputField.text;
        string email = emailInputField.text;
        Sprite sprite = userImage.sprite;
        bool isValued = true;

        if (name.Length < 1)
        {
            nameInputField.image.color = Color.red;
            isValued = false;
        }
        if (phoneNum.Length < 1)
        {
            phonNumInputField.image.color = Color.red;
            isValued = false;
        }
        if (!CheckEamil(email))
        {
            emailInputField.image.color = Color.red;
            isValued = false;
        }
        if (isValued)
        {
            Contact newContact = new Contact();
            newContact.name = name;
            newContact.phoneNumber = phoneNum;
            newContact.email = emailInputField.text;

            if (userImage.sprite)
                newContact.photoName = userImage.sprite.name;
            
            saveDelegate?.Invoke
                (newContact);
            ToggleSetEditMode(true);
        }
    }
    bool CheckEamil(string emailStr)
    {
        Regex regex = new Regex(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]");
        if (regex.IsMatch(emailStr))
        {
            return true;
        }
        return false;
    }   
    public void PeekPopup()
    {
        PeekPicturePopupViewManager peekPicturePopupViewManager
            = Instantiate(PeekPopupViewPrefab, mainManager.transform.parent.parent).GetComponent<PeekPicturePopupViewManager>();
        peekPicturePopupViewManager.PeekPictureOpne();
    }
}