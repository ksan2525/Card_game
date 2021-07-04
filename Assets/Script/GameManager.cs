using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;

    bool isPlayerTurn = true;
    List<int> deck = new List<int>() { 1, 2, 3, 1, 1, 2, 2, 3, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

    private void Start()
    {
        StartGame();
    }

    void StartGame()//�����l�̐ݒ�
    {
        //������D��z��
        SetStartHand();
        //�^�[���̌���
        TurnCalc();


    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }

    void DrowCard(Transform hand)//�J�[�h������
    {
        //�f�b�L���Ȃ��Ȃ�����Ȃ�
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if(playerHandCardList.Length < 6)
        {
            //�f�b�L�̈�ԏ�̃J�[�h��؂���A��D�ɉ�����
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }
        
    }

    void SetStartHand()//��h�ł��O���z��
    {
        for (int i = 0; i < 3; i++)
        {
            DrowCard(playerHand);
        }
    }

    void TurnCalc()//�^�[�����Ǘ�����
    {
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            EnemyTurn();
        }
    }

    public void ChangeTurn()//�^�[���G���h�{�^���ɂ��鏈��
    {
        isPlayerTurn = !isPlayerTurn;//�^�[�����t�ɂ���
        TurnCalc();//�^�[���𑊎�ɉ�
    }

    void PlayerTurn()
    {
        Debug.Log("Player�̃^�[��");

        DrowCard(playerHand);//��D���ꖇ������
    }

    void EnemyTurn()
    {
        Debug.Log("Enemy�̃^�[��");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        if(enemyFieldCardList.Length < 5)
        {
            CreateCard(1, enemyField);//�J�[�h������

            
        }
        ChangeTurn();//�^�[���G���h����
    }
}
