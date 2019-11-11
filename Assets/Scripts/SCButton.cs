using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SCButton : MonoBehaviour
{
    Button CachedButton;
    public Button CacheButton
    {
        get
        {
            if (CachedButton == null)
            {
                CachedButton = GetComponent<Button>();
            }
            return CachedButton;
        }
    }

    // 버튼에 타이틀 변경 함수
    public void SetTitle(string title)
    {
        GetComponentInChildren<Text>().text = title;
    }

    // 버튼의 OnClick 이벤트 변경 함수
    public void SetOnClickAction(UnityAction action)
    {
        GetComponent<Button>().onClick.AddListener(action);
    }

    public void SetInterlectable(bool value)
    {
        this.CacheButton.interactable = value;
    }
}
