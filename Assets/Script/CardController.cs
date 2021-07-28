using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;//�J�[�h�̌����ڂ̏���
    public CardModel model;//�J�[�h�̃f�[�^������

    private void Awake()//
    {
        view = GetComponent<CardView>();
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
}
