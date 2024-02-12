using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;

    private bool gameOver = false;

    private void Update()
    {
        if (gameOver)
        {
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ゲームオーバーの非表示設定
        if (!Panel.activeInHierarchy)
        {
            Panel.SetActive(true);
        }

        gameOver = true;
    }
}
