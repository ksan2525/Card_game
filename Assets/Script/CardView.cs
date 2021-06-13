using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, powerText;
    [SerializeField] Image iconImage;

    public void show(CardModel cardModel)//cardModelÇÃÉfÅ[É^éÊìæÇ∆îΩâf
    {
        nameText.text = cardModel.name;
        powerText.text = cardModel.power.ToString();
        iconImage.sprite = cardModel.icon;
    }
}
