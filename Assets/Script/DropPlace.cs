using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) // ドロップされた時に行う処理
    {
        
        CardController card = eventData.pointerDrag.GetComponent<CardController>(); 

        if (card != null) // もしカードがあれば、
        {
            if(card.model.canUse == true)
            {
                card.movement.cardParent = this.transform; // カードの親要素を自分（アタッチされてるオブジェクト）にする 
                card.DropField(); // カードをフィールドに置いた時の処理を行う
            }

        }
    }
}