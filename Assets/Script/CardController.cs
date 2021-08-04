using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;//�J�[�h�̌����ڂ̏���
    public CardModel model;//�J�[�h�̃f�[�^������
    public CardMovement movement;  // �ړ�(movement)�Ɋւ��邱�Ƃ𑀍�

    private void Awake()//
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID, bool playerCard)//�J�[�h�𐶐������Ƃ��ɌĂ΂��֐�
    {
        model = new CardModel(cardID, playerCard);//�J�[�h�f�[�^�𐶐�
        view.show(model);//�\��
    }

    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }

    public void DropField()
    {
        GameManager.instance.ReduceManaPoint(model.cost);
        model.FieldCard = true;//�t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        model.canUse = false;
        view.SetCanUsePanel(model.canUse);//�o��������CanUsePanel������
    }
}
