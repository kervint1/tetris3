using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;//�V�[���J�ڂ̃��C�u����
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    //�ϐ��̍쐬//
    Spawner spawner;//�X�|�i�[
    Block activeBlock;//�������ꂽ�u���b�N�i�[
    

    
    [SerializeField]
    private float dropInterval = 0.25f;//���Ƀu���b�N��������܂ł̃C���^�[�o������
    float nextdropTimer;//���Ƀu���b�N��������܂ł̎���

    //�ϐ��̍쐬//
    //�{�[�h�̃X�N���v�g���i�[
    Board board;


    //�ϐ��̍쐬//
    //���͎�t�^�C�}�[�i�R��ށj
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;


    //���̓C���^�[�o���i�R��ށj
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;


    //�p�l���̊i�[
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject ControllerPanel;

    //�Q�[���I�[�o�[����
    bool gameOver;

    bool clickLeft = false;
    bool clickRight = false;
    bool clickRotate = false;
    public bool clickDown = false;

    public void Start()
    {
        //�X�|�i�[�I�u�W�F�N�g���X�|�i�[�ϐ��Ɋi�[����R�[�h�̋L�q
        spawner = GameObject.FindObjectOfType<Spawner>();

        //�{�[�h�̕ϐ��Ɋi�[����
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Rounding.Round(spawner.transform.position);


        //�^�C�}�[�̏����ݒ�
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;




        //�X�|�i�[�N���X����u���b�N�����֐���ǂ�ŕϐ��Ɋi�[����
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //�Q�[���I�[�o�[�̔�\���ݒ�
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


    //�֐��̍쐬//
    //�L�[�̓��͂����m���ău���b�N�𓮂����֐�


    void PlayerInput()
    {
        if ((Input.GetKey(KeyCode.D) || clickRight) && (Time.time > nextKeyLeftRightTimer)
            || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();//�E�ɓ�����

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
            activeBlock.MoveLeft();//���ɓ�����

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
            
            activeBlock.MoveDown();//���ɓ�����

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
                    //��ɂ������̏���
                    BottomBoard();
                }

            }
        }
    }

    //�{�[�h�̒�ɒ��������Ɏ��̃u���b�N�𐶐�����֐�
    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();//���܂��Ă���΍폜����
    }

    //�Q�[���I�[�o�[�ɂȂ�����p�l����\������
    void GameOver()
    {
        activeBlock.MoveUp();

        //�Q�[���I�[�o�[�̔�\���ݒ�
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;                
    }

    //�V�[�����ēǂݍ��݂���i�{�^�������ŌĂԁj
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
