using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;//シーン遷移のライブラリ
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    //変数の作成//
    Spawner spawner;//スポナー
    Block activeBlock;//生成されたブロック格納
    

    
    [SerializeField]
    private float dropInterval = 0.25f;//次にブロックが落ちるまでのインターバル時間
    float nextdropTimer;//次にブロックが落ちるまでの時間

    //変数の作成//
    //ボードのスクリプトを格納
    Board board;


    //変数の作成//
    //入力受付タイマー（３種類）
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;


    //入力インターバル（３種類）
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;


    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject ControllerPanel;

    //ゲームオーバー判定
    bool gameOver;

    bool clickLeft = false;
    bool clickRight = false;
    bool clickRotate = false;
    public bool clickDown = false;

    public void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納するコードの記述
        spawner = GameObject.FindObjectOfType<Spawner>();

        //ボードの変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);


        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;




        //スポナークラスからブロック生成関数を読んで変数に格納する
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //ゲームオーバーの非表示設定
        if (gameOverPanel.activeInHierarchy )
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        PlayerInput();
    }


    //関数の作成//
    //キーの入力を検知してブロックを動かす関数


    void PlayerInput()
    {
        if ((Input.GetKey(KeyCode.D) || clickRight) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();//右に動かす

            clickRight = false;

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if ((Input.GetKey(KeyCode.A) || clickLeft) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();//左に動かす

            clickLeft = false;

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if ((Input.GetKey(KeyCode.E) || clickRotate) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            clickRotate = false;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if ((Input.GetKey(KeyCode.S) || clickDown) && (Time.time > nextKeyDownTimer)
            || (Time.time > nextdropTimer))
        {
            
            activeBlock.MoveDown();//下に動かす

            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if (!board.CheckPosition(activeBlock))
            {
                if (board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                    //底についた時の処理
                    BottomBoard();
                }

            }
        }
    }

    //ボードの底に着いた時に次のブロックを生成する関数
    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();//埋まっていれば削除する
    }

    //ゲームオーバーになったらパネルを表示する
    void GameOver()
    {
        activeBlock.MoveUp();

        //ゲームオーバーの非表示設定
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;                
    }

    //シーンを再読み込みする（ボタン押しで呼ぶ）
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void IsLeftClick()
    {
        clickLeft = true;
    }

    public void IsRightClick()
    {
        clickRight = true;
    }
    public void IsRotateClick()
    {
        clickRotate = true;
    }

    public void IsDownClick()
    {
        clickDown = true;
    }
    public void IsUpClick()
    {
        clickDown = false;
    }
}
