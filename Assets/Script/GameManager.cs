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
            //ŽèŽD‚ðˆê–‡”z‚é(Ž©•ª)
            Instantiate(cardPrefab, playerHand);
        }

        


    }

}
