using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view;//カードの見た目の処理
    public CardModel model;//カードのデータを処理

    private void Awake()//
    {
        view = GetComponent<CardView>();
    }

    public void Init(int cardID, bool playerCard)//カードを生成したときに呼ばれる関数
    {
        model = new CardModel(cardID, playerCard);//カードデータを生成
        view.show(model);//表示
    }

    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }
}
