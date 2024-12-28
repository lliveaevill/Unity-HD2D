using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public string enemyName = "Goblin"; // 敵の名前
    public int maxHP = 50; // 最大HP
    public int attackDamage = 10; // 攻撃力
    public float attackDelay = 5f; // 攻撃間隔

    private int currentHP; // 現在のHP

    public TMP_Text enemyNameDisplay; // 敵の名前表示用
    public TMP_Text enemyHPDisplay; // 敵のHP表示用
     private bool isAttacking = false; // 攻撃中かどうか

    private BattleSystem battleSystem; // バトルシステムへの参照
     private float hitChance = 0.5f; // 攻撃が当たる確率（80%）

    void Start()
    {
        currentHP = maxHP; // HPを初期化
        UpdateEnemyDisplay();

        // バトルシステムへの参照を取得
        battleSystem = FindObjectOfType<BattleSystem>();
    }

    // 敵がプレイヤーを攻撃する
   public IEnumerator EnemyAttackRoutine()
{
    isAttacking = true;
     yield return new WaitForSeconds(attackDelay); // 遅延時間を待機

    if (battleSystem.IsBattleActive && currentHP > 0)
        {
            bool attackHit = DetermineAttackHit(); // 攻撃が当たるかどうかを判定

            if (attackHit)
            {
                battleSystem.TakePlayerDamage(attackDamage); // プレイヤーにダメージ
                Debug.Log("Enemy's attack hit!");
            }
            else
            {
                Debug.Log("Enemy's attack missed!");
            }
        }
    isAttacking = false;
     yield return null;
}
 private bool DetermineAttackHit()
    {
        float randomValue = Random.value; // 0〜1のランダムな値を生成
        return randomValue <= hitChance; // ランダム値がhitChance以下なら攻撃が当たる
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
       currentHP -= damage; // ダメージを減算
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HPが負にならないようにする

        Debug.Log($"{enemyName} took {damage} damage! Current HP: {currentHP}");

        UpdateEnemyDisplay(); // HPの表示を更新

        if (currentHP <= 0)
        {
            Die(); // HPが0以下なら死亡処理
        }
    }

    // 敵が倒されたときの処理
    public void Die()
    {
        Debug.Log($"{enemyName} was defeated!");
         FindObjectOfType<GameController>().EndBattle(true);
        Destroy(gameObject, 1f);
    }
    private void NotifyDestruction()
{
    // 例えば、`taiping` スクリプトに通知する仕組みを追加
    if (FindObjectOfType<taiping>() != null)
    {
        FindObjectOfType<taiping>().ClearTargetEnemy(); // 参照を解除
    }
}

    // 敵の表示を更新
    private void UpdateEnemyDisplay()
    {
        if (enemyNameDisplay != null)
        {
            enemyNameDisplay.text = enemyName; // 敵の名前を表示
        }

        if (enemyHPDisplay != null)
        {
            enemyHPDisplay.text = $"HP: {currentHP}/{maxHP}"; // 敵の現在のHPを表示
        }
    }

    // 敵のHPを取得するためのプロパティ
    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;
}