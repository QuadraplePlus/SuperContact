using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

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
    public Sprite sprite;

    public int Compare(Contact x, Contact y)
    {
        return x.name.CompareTo(y.name);
    }

}
