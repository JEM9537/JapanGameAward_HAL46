using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//�ړ����x
    public int jumpflame;
    private PillerManager piller;//�����

    //�t�B�[���h�擾
    Field field;

    public bool InputState;//�ړ����

    private const int BLOCK_NONE = 0;
    private const int BLOCK_UP = 1;
    private const int BLOCK_DOWN = 2;
    private int BlockUpDownFlag;//�u���b�N��艺��t���O
    private bool Way;

    private const int MOVE_NONE = 0;
    private const int MOVE_UP = 1;
    private const int MOVE_X = 2;

    private const int MOVE_CENTER = 3;
    private const int MOVE_DOWN = 4;
    public int MoveFlag;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Manager");
        piller = manager.GetComponent<PillerManager>();


        field = this.GetComponent<Field>();
        field.DefoMoveFlag = true;

        InputState = true;
    }

    // Update is called once per frame
    void Update()
    {
        //���͏���
        ProcesInput();
    }

    private void FixedUpdate()
    {
        //��ړ�
        UpDownMove();

        
    }

    //���͏���
    private void ProcesInput()
    {
        if (!piller.StateReverce() && InputState)
        {
            field.SetNoMove();

            if (Input.GetButton("Right"))//�E
            {
                field.SetMove(-speed);
            }
            else if (Input.GetButton("Left"))//��
            {
                field.SetMove(speed);
            }
            else if (Input.GetButtonDown("Reverce"))//��]
            {
                piller.ReverseStart(this.GetComponent<Field>().nowPiller, false);
            }
            else if (Input.GetButtonDown("Jump"))//�u���b�N���
            {
                BlockUp();
            }
        }
    }


    //�u���b�N����
    private void BlockUp()
    {
        //��Ƀu���b�N�����邩
        if (piller.GetPillerBlock(field.nowPiller, field.nowHeight + 1))
        {
            return;
        }

        if (field.NowWay())//�E�ɂ���ꍇ
        {
            //�E���Ƀu���b�N�����邩 //�E���Ƀu���b�N�����邩
            if (!piller.GetPillerBlock(field.MovePillerID(true), field.nowHeight) ||
                piller.GetPillerBlock(field.MovePillerID(true), field.nowHeight + 1))
            {
                return;
            }

            Way = true;
        }
        else//���ɂ���ꍇ
        {
            //�����Ƀu���b�N�����邩  //�����Ƀu���b�N�����邩
            if (!piller.GetPillerBlock(field.MovePillerID(false), field.nowHeight)||
                piller.GetPillerBlock(field.MovePillerID(false), field.nowHeight + 1))
            {
                return;
            }

            Way = false;
        }


        BlockUpDownFlag = BLOCK_UP;
        InputState = false;
    }

    private void UpDownMove()
    {
        if (piller.StateReverce())
        {
            return;
        }

        if (BlockUpDownFlag == BLOCK_UP)
        {
            if (MoveFlag == MOVE_NONE)//�o��J�n
            {
                field.SetYMove(field.nowHeight + 1, jumpflame);
                MoveFlag = MOVE_UP;
            }
            else if (MoveFlag == MOVE_UP && !field.YMoveFlag)//�o�蒆
            {
                field.SetXMove(Way, jumpflame);//�E�ړ��J�n
                MoveFlag = MOVE_X;
            }
            else if (MoveFlag == MOVE_X && !field.XMoveFlag)//�E�ړ���
            {
                //�u���b�N�o��I���
                InputState = true;
                MoveFlag = MOVE_NONE;
                BlockUpDownFlag = BLOCK_NONE;
            }
        }
        else if (BlockUpDownFlag == BLOCK_DOWN)
        {

            if (MoveFlag == MOVE_NONE && 
                !piller.GetPillerBlock(field.nowPiller, field.nowHeight - 1))//�����Ɉړ��J�n
            {
                field.SetCenterMove(jumpflame);
                //field.SetXMove(true, jumpflame);
                MoveFlag = MOVE_CENTER;
                InputState = false;
            }
            else if (MoveFlag == MOVE_CENTER && !field.MoveCenter)//�����Ɉړ���
            {
                if (field.nowHeight - 1 < 0)//0��菬�����ꍇ
                {
                    MoveFlag = MOVE_NONE;
                    BlockUpDownFlag = BLOCK_NONE;
                    InputState = true;

                }
                else
                {
                    int downnum = 1;
                    for (int i = 2; i < 4; i++)
                    {
                        if (field.nowHeight - i < 0)
                        {
                            break;
                        }
                        else if (!piller.GetPillerBlock(field.nowPiller, field.nowHeight - i))
                        {
                            downnum++;
                        }
                    }
                    field.SelectChangeHeight(field.nowHeight - downnum);
                    MoveFlag = MOVE_DOWN;
                }
            }
            else if (MoveFlag == MOVE_DOWN && this.GetComponent<Rigidbody>().isKinematic)//�󒆂ɂ��鎞
            {
                //���n������
                MoveFlag = MOVE_NONE;
                BlockUpDownFlag = BLOCK_NONE;
                InputState = true;
            }
        }
        else if (BlockUpDownFlag == BLOCK_NONE && field.nowHeight > 0)//������0�ȏ�̎�
        {
            BlockUpDownFlag = BLOCK_DOWN;
        }
        
    }

    //�u���b�N�~���
    private void BlockDown()
    {

    }

    
}
