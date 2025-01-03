using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    public TMP_Text playerNameDisplay; // プレイヤーの名前表示
    public TMP_Text playerHPDisplay; // プレイヤーのHP表示

    public string playerName = "Hero";
    public int playerMaxHP = 100;
    private int playerCurrentHP;

    public bool IsBattleActive { get; private set; } = false; // 戦闘中かどうか
     private EnemyController enemyController; // 敵のコントローラーへの参照

    void Start()
    {
        playerCurrentHP = playerMaxHP; // プレイヤーHP初期化
        playerNameDisplay.text = playerName;
        UpdatePlayerHPDisplay();

        IsBattleActive = true; // 戦闘開始
         // 敵コントローラーの参照を取得
        enemyController = FindObjectOfType<EnemyController>();
    }
     public void InitializeBattle(GameObject player, EnemyController enemyController)
    {
        IsBattleActive = true;

        // 戦闘UIを有効化
        HpDisplay1();

        Debug.Log("Battle initialized with enemy: " + enemyController.name);
    }

    // プレイヤーがダメージを受けたときの処理
    public void TakePlayerDamage(int damage)
    {
        playerCurrentHP -= damage;
        playerCurrentHP = Mathf.Clamp(playerCurrentHP, 0, playerMaxHP); // HPが負にならないように

        UpdatePlayerHPDisplay();

        if (playerCurrentHP <= 0)
        {
            EndBattle(false); // プレイヤー敗北
            
            SceneManager.LoadScene("Sample");
        }
    }
    public void HealPlayer(int healAmount)
{
    playerCurrentHP += healAmount;
    playerCurrentHP = Mathf.Clamp(playerCurrentHP, 0, playerMaxHP); // 最大HPを超えないように制限

    UpdatePlayerHPDisplay();
    Debug.Log($"Player healed by {healAmount} HP!");
}
    public void HpDisplay(){
        playerNameDisplay.gameObject.SetActive(false);
        playerHPDisplay.gameObject.SetActive(false);

    }

    // プレイヤーのHP表示を更新
    private void UpdatePlayerHPDisplay()
    {
        playerHPDisplay.text = $"HP: {playerCurrentHP}/{playerMaxHP}";
    }
    public void HpDisplay1(){
        playerNameDisplay.gameObject.SetActive(true);
        playerHPDisplay.gameObject.SetActive(true);
    }

    // 戦闘終了処理
    private void EndBattle(bool playerWon)
    {
        IsBattleActive = false;
        Debug.Log(playerWon ? "You Won!" : "You Lost...");
        // ゲームコントローラーに戦闘終了を通知
    FindObjectOfType<GameController>().EndBattle(playerWon);
    }
     public void StopEnemyAttack()
    {
        if (enemyController != null)
        {
            StopCoroutine(enemyController.EnemyAttackRoutine()); // 攻撃を停止
        }
    }

    // 敵の攻撃を再開
    public void StartEnemyAttack()
    {
        if (enemyController != null)
        {
            StartCoroutine(enemyController.EnemyAttackRoutine()); // 攻撃を再開
        }
    }
    
}
