using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    public CardModel cardmodel;

    public void OnBeginDrag(PointerEventData eventData) // �h���b�O���n�߂�Ƃ��ɍs������
    {
        
            cardParent = transform.parent;
            transform.SetParent(cardParent.parent, false);
            GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycasts���I�t�ɂ���
        
        
    }

    public void OnDrag(PointerEventData eventData) // �h���b�O�������ɋN��������
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // �J�[�h�𗣂����Ƃ��ɍs������
    {
        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycasts���I���ɂ���
    }
}