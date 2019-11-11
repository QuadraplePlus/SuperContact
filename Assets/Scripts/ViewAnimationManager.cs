using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewAnimationManager : MonoBehaviour
{
    [SerializeField] DetatilViewManager detatilViewManager;
    [SerializeField] ScrollViewManager scrollViewManager;
    Animator animator;
    Animator _animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ScrollViewOn()
    {
        animator.SetTrigger("ScrollOn");
    }
    public void ScrollViewOff()
    {
        animator.SetTrigger("ScrollOff");
    }
}
