using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class taiping : MonoBehaviour
{
   public Button fightButton; // 戦うボタン
    public TMP_InputField inputField; // タイピング入力フィールド
    public TMP_Text resultDisplay; // 結果表示テキスト
    public TMP_Text timerDisplay; // タイマー表示テキスト

    public EnemyController targetEnemy; // 攻撃対象の敵

    // 魔法データ
    private string[] magicWords = { "fireball", "iceblast", "sander", "heal" }; // 登録された魔法の単語
    private int[] magicDamage = { 10, 20, 30, -10 }; // 魔法ごとの攻撃力

    private bool isTyping = false; // タイピング中かどうか
    private bool isCooldown = false; // クールダウン中かどうか
    private float typingTimeLimit = 10f; // タイピングの制限時間
    private float currentTimeLeft; // 残り時間
    private float cooldownTime = 5f; // クールタイムの長さ
    public BattleSystem battleSystem; // BattleSystem を参照するためのフィールド

    private Queue<string> magicQueue = new Queue<string>(); // 入力された魔法を保持するキュー

    void Start()
    {
        fightButton.onClick.AddListener(StartTyping); // 戦うボタンを押すとタイピング開始
    inputField.gameObject.SetActive(false); // 初期は非表示
    timerDisplay.gameObject.SetActive(false); // 初期は非表示

    inputField.onSubmit.RemoveAllListeners(); // リスナーをクリア
    inputField.onSubmit.AddListener(HandleEnterKey); // エンターキーで入力処理
    battleSystem = FindObjectOfType<BattleSystem>();
    }

    void Update()
    {
        if (isTyping)
        {
            currentTimeLeft -= Time.deltaTime;
            timerDisplay.text = $"Time: {Mathf.Clamp(currentTimeLeft, 0, typingTimeLimit):F1}";

            if (currentTimeLeft <= 0)
            {
                EndTypingSession(); // 時間切れでタイピング終了
                if (battleSystem != null)
                {
                    battleSystem.HpDisplay1();
                }
            }
        }
    }

    public void StartTyping()
    {
        if (isCooldown)
        {
            Debug.Log("Cooldown in progress. Wait before starting again.");
            return;
        }

        if (battleSystem != null)
        {
            battleSystem.HpDisplay(); // プレイヤーの名前とHPを非表示にする
            battleSystem.StopEnemyAttack(); // タイピング中は敵の攻撃を止める
        }

        Camera.main.GetComponent<CameraController>().FocusOnEnemy();

        // UIとタイピングの初期化
        inputField.gameObject.SetActive(true);
        timerDisplay.gameObject.SetActive(true);
        inputField.text = ""; // 入力フィールドをクリア
        currentTimeLeft = typingTimeLimit; // 制限時間リセット
        isTyping = true;

        resultDisplay.text = $"spell:"; // タイピング指示を表示
        resultDisplay.gameObject.SetActive(true);

        inputField.ActivateInputField(); // 入力フィールドをフォーカス
        fightButton.interactable = false; // ボタンを無効化
    }

    void HandleEnterKey(string input)
    {
        if (!isTyping) return;

        // 入力が魔法配列の中にあるか確認
        int magicIndex = System.Array.IndexOf(magicWords, input.ToLower());

        if (magicIndex != -1)
        {
            // 正しい魔法をキューに追加
            magicQueue.Enqueue(input.ToLower());
            resultDisplay.text = $" {input.ToLower()} ";
            inputField.text = "";
            inputField.ActivateInputField(); // 再度フォーカス
        }
        else
        {
            // 入力が不正の場合
            resultDisplay.text = $"Spell failed! Try again.";
            inputField.text = "";
            inputField.ActivateInputField(); // 再度フォーカス
        }
    }

    void EndTypingSession()
    {
        isTyping = false;
        inputField.text = "";
        inputField.gameObject.SetActive(false);
        timerDisplay.gameObject.SetActive(false);

        if (magicQueue.Count > 0)
        {
            StartCoroutine(ProcessMagicQueue());
        }
        else
        {
            resultDisplay.text = $"Time's up!";
            StartCoroutine(HideResultDisplayAfterDelay(2f));
        }
         if (targetEnemy != null && targetEnemy.CurrentHP <= 0)
        {
            FindObjectOfType<GameController>().EndBattle(true);
        }
        else
        {
            if (battleSystem != null)
            {
                battleSystem.StartEnemyAttack();
            }
            StartCoroutine(CooldownRoutine()); // クールタイム開始
        }

        fightButton.interactable = true;
    }

    private IEnumerator ProcessMagicQueue()
    {
        while (magicQueue.Count > 0)
        {
            string magic = magicQueue.Dequeue();
            int magicIndex = System.Array.IndexOf(magicWords, magic);
            int effect = magicDamage[magicIndex];

            if (effect < 0)
            {
                // 回復魔法の場合
                resultDisplay.text = $"{magic} healed {Mathf.Abs(effect)} HP!";
                battleSystem.HealPlayer(Mathf.Abs(effect));
            }
            else
            {
                // 攻撃魔法の場合
                resultDisplay.text = $"{magic} dealt {effect} damage!";
                if (targetEnemy != null)
                {
                    targetEnemy.TakeDamage(effect);
                }
            }

            yield return new WaitForSeconds(1f); // 次の魔法を処理するまで1秒待機
        }

        resultDisplay.text = $"All spells executed!";
        StartCoroutine(HideResultDisplayAfterDelay(2f));

        // if (targetEnemy != null && targetEnemy.CurrentHP <= 0)
        // {
        //     FindObjectOfType<GameController>().EndBattle(true);
        // }
        // else
        // {
        //     if (battleSystem != null)
        //     {
        //         battleSystem.StartEnemyAttack();
        //     }
        //     StartCoroutine(CooldownRoutine()); // クールタイム開始
        // }
    }

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime); // クールタイム待機
        isCooldown = false;
        fightButton.interactable = true; // ボタンを再度有効化
    }

    private IEnumerator HideResultDisplayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultDisplay.gameObject.SetActive(false); // 結果表示を非表示
    }

    public void ClearTargetEnemy()
    {
        targetEnemy = null; // 敵参照を解除
    }
}