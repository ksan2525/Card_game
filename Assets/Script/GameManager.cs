using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;
    [SerializeField] Text playerManaPointText;
    [SerializeField] Text playerDefaultManaPointText;

    public int playerManaPoint; // �g�p����ƌ���}�i�|�C���g
    public int playerDefaultManaPoint; // ���^�[�������Ă����x�[�X�̃}�i�|�C���g
    public GameObject endbutton;
    public Transform enemyattackcard;

    bool isPlayerTurn = true;

    List<int> deck = new List<int>() { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, };
    List<int> enemydeck = new List<int>() { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, };


    private void Start()
    {
        StartGame();
    }

    void Shuffle()//�f�b�L���V���b�t������
    {
        Debug.Log("�f�b�L���V���b�t��������I");
        //���� n �̏����l�̓f�b�L�̖���
        int n = deck.Count;

        //n��1��菬�����Ȃ�܂ŌJ��Ԃ�
        while (n > 1)
        {
            n--;

            //k��0 �` n+1 �̃����_���Ȓl
            int k = UnityEngine.Random.Range(0, n + 1);

            //k�Ԗڂ̃J�[�h��temp�ɑ��
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void enemyShuffle()//�f�b�L���V���b�t������
    {
        Debug.Log("�f�b�L���V���b�t��������I");
        //���� n �̏����l�̓f�b�L�̖���
        int n = enemydeck.Count;

        //n��1��菬�����Ȃ�܂ŌJ��Ԃ�
        while (n > 1)
        {
            n--;

            //k��0 �` n+1 �̃����_���Ȓl
            int k = UnityEngine.Random.Range(0, n + 1);

            //k�Ԗڂ̃J�[�h��temp�ɑ��
            int temp = enemydeck[k];
            enemydeck[k] = enemydeck[n];
            enemydeck[n] = temp;
        }
    }

    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void StartGame()//�����l�̐ݒ�
    {

        
        Shuffle();
        enemyShuffle();

        enemyLeaderHP = 5000;
        playerLeaderHP = 50000;
        ShowLeaderHP();

        /// �}�i�̏����l�ݒ� ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        //������D��z��
        SetStartHand();
        //�^�[���̌���
        TurnCalc();

    }

    void ShowManaPoint() // �}�i�|�C���g��\�����郁�\�b�h
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
    }

    public void ReduceManaPoint(int cost)//�R�X�g�̕��A�}�i�|�C���g�����炷
    {
        playerManaPoint -= cost;
        ShowManaPoint();

        SetCanUsePanelHand();
    }

    void SetCanUsePanelHand() // ��D�̃J�[�h���擾���āA�g�p�\�ȃJ�[�h��CanUse�p�l����t����
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerHandCardList)
        {
            if (card.model.cost <= playerManaPoint)
            {
                card.model.canUse = true;
                card.view.SetCanUsePanel(card.model.canUse);
            }
            else
            {
                card.model.canUse = false;
                card.view.SetCanUsePanel(card.model.canUse);
            }
        }
    }
    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);

        // Player�̎�D�ɐ������ꂽ�J�[�h��Player�̃J�[�h�Ƃ���
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

        SetCanUsePanelHand();
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
            //EnemyTurn();
            StartCoroutine(EnemyTurn());//StartCoroutine�ŌĂяo��
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
        endbutton.SetActive(true);

        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        //�}�i�𑝂₷
        playerDefaultManaPoint++;
        playerManaPoint = playerDefaultManaPoint;
        ShowManaPoint();

        DrowCard(playerHand);//��D���ꖇ������

    }

    IEnumerator EnemyTurn()//StartCoroutine�ŌĂ΂ꂽ�̂ŁAIEnnumerator�ɕύX
    {
        endbutton.SetActive(false);

        Debug.Log("Enemy�̃^�[��");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        //�G�̃t�B�[���h�̃J�[�h���U���\�ɂ��āA�΂̘g������
        SetAttackableFieldCard(enemyFieldCardList, true);

        yield return new WaitForSeconds(1f);

        if (enemyFieldCardList.Length < 5)
        {
            int cardID = enemydeck[0];
            enemydeck.RemoveAt(0);
            CreateCard(cardID, enemyField);
        }

        CardController[] enemyFieldCardListSecond = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        while (Array.Exists(enemyFieldCardListSecond, card => card.model.canAttack))
        {
            //�U���\�J�[�h���擾
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardListSecond, card => card.model.canAttack);
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();

            

            int randomattack = UnityEngine.Random.Range(0, playerFieldCardList.Length);
            CardController attackCard = enemyCanAttackCardList[0];

            string kougekicard = playerFieldCardList[randomattack].ToString();
            //�U������J�[�h�̍��W���擾
            enemyattackcard = GameObject.Find(kougekicard).transform;

            Debug.Log(enemyattackcard.transform.position);



            if (playerFieldCardList.Length > 0)//�v���C���[�m�o�ɃJ�[�h������ꍇ
            {
                
                CardController defenceCard = playerFieldCardList[randomattack];
                CardBattle(attackCard, defenceCard);
            }
            else//�v���C���[�̏�ɃJ�[�h���Ȃ��ꍇ
            {
                AttackToLeader(attackCard, false);
            }

            yield return new WaitForSeconds(1f);

            enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        }

        yield return new WaitForSeconds(1f);

        ChangeTurn();//�^�[���G���h����
    }

    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // �U���J�[�h�ƍU�������J�[�h�������v���C���[�̃J�[�h�Ȃ�o�g�����Ȃ�
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            return;
        }
        //�U���J�[�h���A�^�b�N�\�łȂ���΍U�����Ȃ��ŏ����I������
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        // �U�����̃p���[�����������ꍇ�A�U�����ꂽ�J�[�h��j�󂷂�
        if (attackCard.model.power > defenceCard.model.power)
        {
            defenceCard.DestroyCard(defenceCard);
        }

        // �U�����ꂽ���̃p���[�����������ꍇ�A�U�����̃J�[�h��j�󂷂�
        if (attackCard.model.power < defenceCard.model.power)
        {
            attackCard.DestroyCard(attackCard);
        }

        // �p���[�������������ꍇ�A�����̃J�[�h��j�󂷂�
        if (attackCard.model.power == defenceCard.model.power)
        {
            attackCard.DestroyCard(attackCard);
            defenceCard.DestroyCard(defenceCard);
        }

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
    }

    void SetAttackableFieldCard(CardController[] cardList, bool canAttack)
    {
        foreach (CardController card in cardList)
        {
            card.model.canAttack = canAttack;
            card.view.SetCanAttackPanel(canAttack);
        }
    }

    public int playerLeaderHP;
    public int enemyLeaderHP;

    public void AttackToLeader(CardController attackCard, bool isPlayerCard)
    {
        if (attackCard.model.canAttack == false)
        {
            return;
        }

        if (attackCard.model.PlayerCard == true)//attackCard���v���C���[�̃J�[�h�Ȃ�
        {
            enemyLeaderHP -= attackCard.model.power;//�G�̃��[�_�[��HP�����炷
        }
        else//attackCard���G�̃J�[�h�Ȃ�
        {
            playerLeaderHP -= attackCard.model.power;//�v���C���[�̃��[�_�[��HP�����炷
        }

        //enemyLeaderHP -= attackCard.model.power;

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("�G��HP�́A" + enemyLeaderHP);
        ShowLeaderHP();
    }

    public void ShowLeaderHP()
    {
        if (playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
        }
        if (enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
        }

        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHPText.text = enemyLeaderHP.ToString();
    }

}
