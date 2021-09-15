using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    bool canDrag = true;//カードを動かせるかどうかのフラグ

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {
        CardController card = GetComponent<CardController>();
        canDrag = true;

        if (card.model.FieldCard == false)//手札のカードなら
        {
            if (card.model.canUse == false)//マナコストより少ないカードは動かせない
            {
                canDrag = false;
            }
        }
        else
        {
            if (card.model.canAttack == false)//攻撃不可能なカードは動かせない
            {
                canDrag = false;
            }
        }

        if (canDrag == false)
        {
            return;
        }

        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
    }

    public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        if (canDrag == false)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        if (canDrag == false)
        {
            return;
        }

        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする
    }

    public void Deckdrow(Transform cardidou)
    {
        transform.DOLocalMove(cardidou.transform.position, 1f);
    }
}