using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PagingManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    ScrollRect chacedScrollRect;
    Coroutine moveCoroutine;

    public ScrollRect ChahedScrollRect
    {
        get
        {
            if (chacedScrollRect == null)
            {
                chacedScrollRect = GetComponent<ScrollRect>();
            }
            return chacedScrollRect;
        }
        set
        {
            this.chacedScrollRect = value;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GridLayoutGroup gridLayoutGroup
            = ChahedScrollRect.content.GetComponent<GridLayoutGroup>();

        ChahedScrollRect.StopMovement();

        float pageWidth = -(gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x);
        int pageIndex = Mathf.RoundToInt(ChahedScrollRect.content.anchoredPosition.x / pageWidth);

        float targetX = pageIndex * pageWidth;

        if (pageIndex >= 0 && pageIndex <= gridLayoutGroup.transform.childCount - 1)
        {
            moveCoroutine = StartCoroutine(LerpMoveCell(new Vector2(targetX, 0), 0.3f));
        }
    }
    public void OnTest(Vector2 value)
    {

    }
    public void OnEnable()
    {

    }
    IEnumerator LerpMoveCell(Vector2 targetPos , float duration)
    {
        Vector2 dragEndPos = ChahedScrollRect.content.anchoredPosition;

        float currentTime = 0;

        while (currentTime < duration)
        {
            Vector2 newPos = dragEndPos + (targetPos - dragEndPos);
            float newTime = currentTime / duration;

            Vector2 destPos = new Vector2(Mathf.Lerp(dragEndPos.x , newPos.x , newTime), newPos.y);

            ChahedScrollRect.content.anchoredPosition = destPos;
            currentTime += Time.deltaTime;

            yield return null;
        }
    }
}
