using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
[RequireComponent(typeof(Animator))]
public class PopupViewManager : MonoBehaviour
{
    Animator animator;

    public enum AnimationType {TYPE1 , TYPE2}

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    // 팝업 나타날때 호출할 함수
    public void Open()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("open");
    }
    protected void Close()
    {
        animator.SetTrigger("close");      
    }
    public void SetDisablePanel()
    {
        Destroy(gameObject);
    }
    public void PeekPictureOpne()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("PeekOpen");
    }
    public void PeekPictureClose()
    {
        animator.SetTrigger("PeekClose");
    }   
}