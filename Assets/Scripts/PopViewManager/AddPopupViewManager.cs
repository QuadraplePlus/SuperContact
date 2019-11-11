using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddPopupViewManager : PopupViewManager
{
    [SerializeField] InputField nameInputField;
    [SerializeField] InputField phoneNumberInputField;
    [SerializeField] InputField emailInputField;
    [SerializeField] Image userImage;
    [SerializeField] GameObject PeekPopupViewPrefab;
    
    public delegate void AddContact(Contact contact);
    public AddContact addContactCallback;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("자식의 Start()");
        PeekPicturePopupViewManager.sendImage = (sprite) =>
        {
            userImage.GetComponent<Image>().sprite = sprite;
        };
    }
    private void OnEnable()
    {
        InitInputField(nameInputField);
        InitInputField(phoneNumberInputField);
        InitInputField(emailInputField);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EventSystem system;
            system = EventSystem.current;
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            else Debug.Log("다음 InputField가 존재하지 않습니다");
        }
    }
    public void OnClickAdd()
    {
        // TODO:
        // InputField의 값을 불러와서 Main 화면으로 값 전달
        string name = nameInputField.text;
        string phoneNumber = phoneNumberInputField.text;
        string email = emailInputField.text;
        
        bool isValid = true;

        if (name.Length < 1)
        {
            nameInputField.image.color = Color.red;
            isValid = false;
        }

        if (phoneNumber.Length < 1)
        {
            phoneNumberInputField.image.color = Color.red;
            isValid = false;
        }

        if (!IsCorrectEmail(email))
        {
            emailInputField.image.color = Color.red;
            isValid = false;
        }

        if (isValid)
        {
            Contact contact = new Contact();
            contact.name = name;
            contact.phoneNumber = phoneNumber;
            contact.email = email;
            if (userImage.sprite)
                contact.photoName = userImage.sprite.name;

            // Main 화면에 Contact 객체 전달
            addContactCallback(contact);

            // AddPanel 닫기
            Close();
        }
    }

    // 취소 버튼
    public void OnClickClose()
    {
        Close();
    }

    // 이메일 형식 체크
    bool IsCorrectEmail(string emailStr)
    {
        Regex regex = new Regex(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]");
        if (regex.IsMatch(emailStr))
        {
            return true;
        }
        return false;
    }

    // InputField의 내용을 초기화
    public void InitInputField(InputField inputField)
    {
        inputField.text = "";
        inputField.image.color = Color.white;
    }
    public void PeekPopup()
    {
        PeekPicturePopupViewManager peekPicturePopupViewManager
            = Instantiate(PeekPopupViewPrefab , transform.parent).GetComponent<PeekPicturePopupViewManager>();
        peekPicturePopupViewManager.PeekPictureOpne();
    }
}