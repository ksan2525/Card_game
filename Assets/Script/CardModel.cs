using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cardID;
    public string name;
    public int cost;
    public int power;
    public Sprite icon;

    public bool PlayerCard = false;

    public bool canAttack = false;
    public CardModel(int cardID, bool playerCard)//データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);//CardEntityのパス

        cardID = cardEntity.cardID;
        name = cardEntity.name;
        cost = cardEntity.cost;
        power = cardEntity.power;
        icon = cardEntity.icon;

        PlayerCard = playerCard;
    }
}
