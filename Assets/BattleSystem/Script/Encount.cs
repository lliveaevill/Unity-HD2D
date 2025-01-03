using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encount : MonoBehaviour
{
      public int stepsToEncounter = 20; // エンカウントまでの歩数
    public float stepThreshold = 0.5f; // 1歩とみなす移動距離

    private int currentSteps = 0;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // プレイヤーの移動を検出
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        
        if (distanceMoved >= stepThreshold)
        {
            currentSteps++;
            lastPosition = transform.position;
            
            Debug.Log("Steps: " + currentSteps);

            // エンカウント条件を満たしたか確認
            if (currentSteps >= stepsToEncounter)
            {
                TriggerEncounter();
                currentSteps = 0; // 歩数をリセット
            }
        }
    }

    private void TriggerEncounter()
    {
        Debug.Log("Encounter triggered!");
        SceneManager.LoadScene("BattleScene");
        // ここにエンカウント時の処理を実装
        // 例: バトルシーンへの遷移、敵の生成など
    }
}
