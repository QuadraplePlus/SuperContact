using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public static class SCInptuField
{
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
    [SerializeField] GameObject thirdView;
    [SerializeField] GameObject PeekPopupViewPrefab;
    //[SerializeField] ThirdViewManager thirdViewManager;

    public delegate void DetailViewManagerSaveDelegate(Contact contact);
    public DetailViewManagerSaveDelegate saveDelegate;

    public Contact? contact;
    EventSystem system;
    Animator animator;
    
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
        PeekPicturePopupViewManager.sendImage = (sprite) =>
        {
            userImage.sprite = sprite;
        };
    }
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
        animator = GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        Destroy(rightNavgationViewButton.gameObject);
    }

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
            newContact.sprite = userImage.sprite;
            
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
            = Instantiate(PeekPopupViewPrefab, mainManager.transform).GetComponent<PeekPicturePopupViewManager>();
        peekPicturePopupViewManager.PeekPictureOpne();
    }
}