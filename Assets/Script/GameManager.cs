using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand,playerField;

    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        
            CreateCard(1, playerHand);
            CreateCard(2, playerHand);
            CreateCard(2, playerHand);
            CreateCard(1, playerHand);
            CreateCard(2, playerHand);
            CreateCard(1, playerHand);
        

    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }
}
