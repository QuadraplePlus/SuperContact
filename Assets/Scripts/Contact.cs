using System.Collections.Generic;
//JSON 파일에 문자열들을 한번에 저장하기 위한 Contact의 리스트
public struct Contacts
{
    public List<Contact> contactList;

    public Contacts(List<Contact> contacts)
    {
        this.contactList = contacts;
    }
}

[System.Serializable]
public struct Contact : IComparer<Contact>
{
    public string name; 
    public string phoneNumber;
    public string email;
    public string photoName;
    //문자열 순서 대로 소팅을 위한 함수
    public int Compare(Contact x, Contact y)
    {
        return x.name.CompareTo(y.name);
    }

}
