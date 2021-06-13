using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cartID;
    public string name;
    public int power;
    public Sprite icon;
    public CardModel(int cardID)//データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);//CardEntityのパス

        cardID = cardEntity.cardID;
        name = cardEntity.name;
        power = cardEntity.power;
        icon = cardEntity.icon;
    }
}
