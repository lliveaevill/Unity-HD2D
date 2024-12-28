using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
     public GameObject[] enemyPrefabs;  // 敵のプレハブ
    public Transform enemySpawnPoint; // 敵のスポーン地点

    void Start()
    {
        // 保存された敵インデックスを取得
        int enemyIndex = PlayerPrefs.GetInt("SelectedEnemyIndex", 0);
        if (enemyIndex >= 0 && enemyIndex < enemyPrefabs.Length)
        {
            // 敵をスポーン
            Instantiate(enemyPrefabs[enemyIndex], enemySpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Invalid enemy index");
        }
    }
    public void Die()
    {
        Debug.Log($" defeated!");
         FindObjectOfType<GameController>().EndBattle(true);
        Destroy(gameObject, 1f);
    }
}
