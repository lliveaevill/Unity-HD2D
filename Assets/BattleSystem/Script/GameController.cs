using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public GameObject player;          // プレイヤーオブジェクト
    public GameObject[] enemyPrefabs;  // 敵のプレハブ
    public Transform[] spawnPoints;    // 敵の出現地点
    private Vector3 playerStartPosition; // プレイヤーの元の位置
    private bool isInBattle = false;   // 戦闘中フラグ
    private string explorationSceneName = "stage1"; // 探索シーン名
    private string battleSceneName = "SampleScene";           // 戦闘シーン名

    void Start()
    {
        playerStartPosition = player.transform.position; // プレイヤーの初期位置を記録
    }

    void Update()
    {
        if (!isInBattle)
        {
            CheckForRandomEncounter();
        }
    }

    void CheckForRandomEncounter()
    {
        if (Random.value < 0.0005f) // 1%の確率でエンカウント
        {
            StartBattle();
        }
    }

    void StartBattle()
    {
        isInBattle = true;

        // ランダムな敵を選択して保存
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        PlayerPrefs.SetInt("SelectedEnemyIndex", enemyIndex);

        // プレイヤーの現在位置を保存
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);

        // 戦闘シーンに移動
        SceneManager.LoadScene(battleSceneName);
    }

    public void EndBattle(bool playerWon)
    {
        isInBattle = false;

        Debug.Log(playerWon ? "Player won the battle!" : "Player lost the battle...");
        
        // 探索シーンに戻る
        SceneManager.LoadScene(explorationSceneName);
    }
}
