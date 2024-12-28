using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;         // プレイヤーのTransform
    public Transform enemy;          // 敵のTransform
    public Transform originalPos;    // カメラの元の位置
    public float moveSpeed = 2f;     // カメラ移動のスピード
    public float lookAtDuration = 5f; // 敵を注視する時間

    private bool isLookingAtEnemy = false; // カメラが敵を注視しているかどうか

    void Start()
    {
        // 現在のカメラ位置を元の位置として記憶
        if (originalPos == null)
        {
            GameObject originalPosObj = new GameObject("CameraOriginalPos");
            originalPosObj.transform.position = transform.position;
            originalPosObj.transform.rotation = transform.rotation;
            originalPos = originalPosObj.transform;
        }
    }

    public void FocusOnEnemy()
    {
        if (enemy == null) return;

        if (!isLookingAtEnemy)
        {
            StartCoroutine(LookAtEnemy());
        }
    }

    private IEnumerator LookAtEnemy()
    {
        isLookingAtEnemy = true;

         // 敵の中心を取得
    // 敵の中心を基準にカメラの目標位置を計算
    Vector3 enemyCenter = enemy.position + Vector3.up * (enemy.localScale.y / 2f); // 敵の中心を計算
    Vector3 targetPosition = enemyCenter - enemy.forward * 15f + Vector3.up * 1.5f;   // 敵の正面位置から少し離れた位置を計算

    // カメラの目標回転を計算 (敵の中心を注視)
    Quaternion targetRotation = Quaternion.LookRotation(enemyCenter - targetPosition);

    // カメラをスムーズに移動させる
    float t = 0f;
    while (t < 1f)
    {
        t += Time.deltaTime * moveSpeed;

        // カメラ位置と回転を補間
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);

        yield return null;
    }

    // 最後に正確な位置と回転を設定
    transform.position = targetPosition;
    transform.rotation = targetRotation;
        // 一定時間敵を注視
        yield return new WaitForSeconds(lookAtDuration);

        // 元の位置に戻る
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(transform.position, originalPos.position, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalPos.rotation, t);
            yield return null;
        }

        isLookingAtEnemy = false;
    }
}