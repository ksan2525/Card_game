using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform playerHand;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            //手札を一枚配る(自分)
            Instantiate(cardPrefab, playerHand);
        }

        


    }

}
