using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;//�J�[�h�̌����ڂ̏���
    public CardModel model;//�J�[�h�̃f�[�^������

    private void Awake()
    {
        view = GetComponent<CardView>();
    }

    public void Init(int cardID)//�J�[�h�𐶐������Ƃ��ɌĂ΂��֐�
    {
        model = new CardModel(cardID);//�J�[�h�f�[�^�𐶐�
        view.show(model);//�\��
    }

}