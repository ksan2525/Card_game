using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;

    bool isPlayerTurn = true;
    List<int> deck = new List<int>() { 1, 2, 3, 1, 1, 2, 2, 3, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };


    private void Start()
    {
        StartGame();
    }

    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void StartGame()//初期値の設定
    {
        enemyLeaderHP = 5000;
        playerLeaderHP = 5000;
        ShowLeaderHP();

        //初期手札を配る
        SetStartHand();
        //ターンの決定
        TurnCalc();


    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);

        // Playerの手札に生成されたカードはPlayerのカードとする
        if (place == playerHand)
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }

    }

    void DrowCard(Transform hand)//カードを引く
    {
        //デッキがないなら引かない
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 9)
        {
            //デッキの一番上のカードを切り取り、手札に加える
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

    }

    void SetStartHand()//手札を三枚配る
    {
        for (int i = 0; i < 3; i++)
        {
            DrowCard(playerHand);
        }
    }

    void TurnCalc()//ターンを管理する
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

    public void ChangeTurn()//ターンエンドボタンにつける処理
    {
        isPlayerTurn = !isPlayerTurn;//ターンを逆にする
        TurnCalc();//ターンを相手に回す
    }

    void PlayerTurn()
    {
        Debug.Log("Playerのターン");

        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        DrowCard(playerHand);//手札を一枚加える
    }

    void EnemyTurn()
    {
        Debug.Log("Enemyのターン");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        if (enemyFieldCardList.Length < 5)
        {
            if (enemyFieldCardList.Length < 2)
            {
                CreateCard(3, enemyField);
            }
            else
            {
                CreateCard(2, enemyField);
            }          
        }
        ChangeTurn();//ターンエンドする
    }

    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // 攻撃カードと攻撃されるカードが同じプレイヤーのカードならバトルしない
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            return;
        }
        //攻撃カードがアタック可能でなければ攻撃しないで処理終了する
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        // 攻撃側のパワーが高かった場合、攻撃されたカードを破壊する
        if (attackCard.model.power > defenceCard.model.power)
        {
            defenceCard.DestroyCard(defenceCard);
        }

        // 攻撃された側のパワーが高かった場合、攻撃側のカードを破壊する
        if (attackCard.model.power < defenceCard.model.power)
        {
            attackCard.DestroyCard(attackCard);
        }

        // パワーが同じだった場合、両方のカードを破壊する
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

        enemyLeaderHP -= attackCard.model.power;

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("敵のHPは、" + enemyLeaderHP);
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
