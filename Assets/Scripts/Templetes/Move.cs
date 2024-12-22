using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f; // 移動速度

    // Update is called once per frame
    void Update()
    {
        // 入力を取得
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 移動ベクトルを計算
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // オブジェクトを移動
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}