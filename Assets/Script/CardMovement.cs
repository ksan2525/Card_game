using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    bool canDrag = true;//�J�[�h�𓮂����邩�ǂ����̃t���O

    public void OnBeginDrag(PointerEventData eventData) // �h���b�O���n�߂�Ƃ��ɍs������
    {
        CardController card = GetComponent<CardController>();
        canDrag = true;

        if (card.model.FieldCard == false)//��D�̃J�[�h�Ȃ�
        {
            if (card.model.canUse == false)//�}�i�R�X�g��菭�Ȃ��J�[�h�͓������Ȃ�
            {
                canDrag = false;
            }
        }
        else
        {
            if (card.model.canAttack == false)//�U���s�\�ȃJ�[�h�͓������Ȃ�
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
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycasts���I�t�ɂ���
    }

    public void OnDrag(PointerEventData eventData) // �h���b�O�������ɋN��������
    {
        if (canDrag == false)
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // �J�[�h�𗣂����Ƃ��ɍs������
    {
        if (canDrag == false)
        {
            return;
        }

        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycasts���I���ɂ���
    }

    public void Deckdrow(Transform cardidou)
    {
        transform.DOLocalMove(cardidou.transform.position, 1f);
    }
}