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

    public int playerManaPoint; // 使用すると減るマナポイント
    public int playerDefaultManaPoint; // 毎ターン増えていくベースのマナポイント
    public GameObject endbutton;
    public Transform enemyattackcard;

    bool isPlayerTurn = true;

    List<int> deck = new List<int>() { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, };
    List<int> enemydeck = new List<int>() { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, };


    private void Start()
    {
        StartGame();
    }

    void Shuffle()//デッキをシャッフルする
    {
        Debug.Log("デッキをシャッフルしたよ！");
        //整数 n の初期値はデッキの枚数
        int n = deck.Count;

        //nが1より小さくなるまで繰り返す
        while (n > 1)
        {
            n--;

            //kは0 ～ n+1 のランダムな値
            int k = UnityEngine.Random.Range(0, n + 1);

            //k番目のカードをtempに代入
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void enemyShuffle()//デッキをシャッフルする
    {
        Debug.Log("デッキをシャッフルしたよ！");
        //整数 n の初期値はデッキの枚数
        int n = enemydeck.Count;

        //nが1より小さくなるまで繰り返す
        while (n > 1)
        {
            n--;

            //kは0 ～ n+1 のランダムな値
            int k = UnityEngine.Random.Range(0, n + 1);

            //k番目のカードをtempに代入
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
    void StartGame()//初期値の設定
    {

        
        Shuffle();
        enemyShuffle();

        enemyLeaderHP = 5000;
        playerLeaderHP = 50000;
        ShowLeaderHP();

        /// マナの初期値設定 ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        //初期手札を配る
        SetStartHand();
        //ターンの決定
        TurnCalc();

    }

    void ShowManaPoint() // マナポイントを表示するメソッド
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
    }

    public void ReduceManaPoint(int cost)//コストの分、マナポイントを減らす
    {
        playerManaPoint -= cost;
        ShowManaPoint();

        SetCanUsePanelHand();
    }

    void SetCanUsePanelHand() // 手札のカードを取得して、使用可能なカードにCanUseパネルを付ける
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

        SetCanUsePanelHand();
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
            //EnemyTurn();
            StartCoroutine(EnemyTurn());//StartCoroutineで呼び出す
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
        endbutton.SetActive(true);

        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        //マナを増やす
        playerDefaultManaPoint++;
        playerManaPoint = playerDefaultManaPoint;
        ShowManaPoint();

        DrowCard(playerHand);//手札を一枚加える

    }

    IEnumerator EnemyTurn()//StartCoroutineで呼ばれたので、IEnnumeratorに変更
    {
        endbutton.SetActive(false);

        Debug.Log("Enemyのターン");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        //敵のフィールドのカードを攻撃可能にして、緑の枠をつける
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
            //攻撃可能カードを取得
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardListSecond, card => card.model.canAttack);
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();

            

            int randomattack = UnityEngine.Random.Range(0, playerFieldCardList.Length);
            CardController attackCard = enemyCanAttackCardList[0];

            string kougekicard = playerFieldCardList[randomattack].ToString();
            //攻撃するカードの座標を取得
            enemyattackcard = GameObject.Find(kougekicard).transform;

            Debug.Log(enemyattackcard.transform.position);



            if (playerFieldCardList.Length > 0)//プレイヤーノバにカードがある場合
            {
                
                CardController defenceCard = playerFieldCardList[randomattack];
                CardBattle(attackCard, defenceCard);
            }
            else//プレイヤーの場にカードがない場合
            {
                AttackToLeader(attackCard, false);
            }

            yield return new WaitForSeconds(1f);

            enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        }

        yield return new WaitForSeconds(1f);

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

        if (attackCard.model.PlayerCard == true)//attackCardがプレイヤーのカードなら
        {
            enemyLeaderHP -= attackCard.model.power;//敵のリーダーのHPを減らす
        }
        else//attackCardが敵のカードなら
        {
            playerLeaderHP -= attackCard.model.power;//プレイヤーのリーダーのHPを減らす
        }

        //enemyLeaderHP -= attackCard.model.power;

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
