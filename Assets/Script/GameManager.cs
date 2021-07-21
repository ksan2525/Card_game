using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;

    bool isPlayerTurn = true;
    List<int> deck = new List<int>() { 1, 2, 3, 1, 1, 2, 2, 3, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
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

        //Player�̎�D�ɐ������ꂽ�J�[�h��Player�̃J�[�h�Ƃ���
        if (place == playerHand)
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }

    }

    void DrowCard(Transform hand)//�J�[�h������
    {
        //�f�b�L���Ȃ��Ȃ�����Ȃ�
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 9)
        {
            //�f�b�L�̈�ԏ�̃J�[�h��؂���A��D�ɉ�����
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

    }

    void SetStartHand()//��D���O���z��
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

        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        DrowCard(playerHand);//��D���ꖇ������
    }

    void EnemyTurn()
    {
        Debug.Log("Enemy�̃^�[��");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        if (enemyFieldCardList.Length < 5)
        {
            CreateCard(1, enemyField);//�J�[�h������


        }
        ChangeTurn();//�^�[���G���h����
    }

    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        //�U���J�[�h�ƍU�������J�[�h�������v���C���[�̃J�[�h�Ȃ�o�g�����Ȃ�
        if(attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            return;
        }
        //�U���J�[�h���A�^�b�N�\�łȂ���΍U�����Ȃ��ŏ����I������
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        //�U�����̃p���[�����������ꍇ�A�U�����ꂽ�J�[�h��j�󂷂�
        if (attackCard.model.power > defenceCard.model.power)
        {
            defenceCard.DestroyCard(defenceCard);
        }

        //�U�����ꂽ���̃p���[�����������ꍇ�A�U�����̃J�[�h��j�󂷂�
        if (attackCard.model.power < defenceCard.model.power)
        {
            attackCard.DestroyCard(attackCard);
        }

        //�p���[�������������ꍇ�A�����̃J�[�h��j�󂷂�
        if (attackCard.model.power == defenceCard.model.power)
        {
            attackCard.DestroyCard(attackCard);
            defenceCard.DestroyCard(defenceCard);
        }

        attackCard.model.canAttack = false;
    }

    void SetAttackableFieldCard(CardController[] cardList,bool canAttack)
    {
        foreach (CardController card in cardList)
        {
            card.model.canAttack = canAttack;
        }
    }
}
