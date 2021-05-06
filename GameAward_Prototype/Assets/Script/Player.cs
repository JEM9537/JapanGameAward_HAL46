using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//�ړ����x
    public int jumpflame;
    private PillerManager piller;//�����
    private TurnPillerManager turnpiller;

    //�t�B�[���h�擾
    Field field;

    private const int BLOCK_NONE = 0;
    private const int BLOCK_UP = 1;
    private const int BLOCK_DOWN = 2;
    private int BlockUpDownFlag = BLOCK_NONE;//�u���b�N��艺��t���O
    private bool Way;

    private const int MOVE_NONE = 0;
    private const int MOVE_UP = 1;
    private const int MOVE_X = 2;
    private const int MOVE_CENTER = 3;
    private const int MOVE_DOWN = 4;
    public int MoveFlag = MOVE_NONE;

    private const int INPUT_NONE = 0;//���͂��ĂȂ�
    private const int INPUT_SET = 1;//���͂��Ă���܂��͉������珈����
    private const int INPUT_LEFT = 2;//��
    private const int INPUT_RIGHT = 3;//�E
    private const int INPUT_REVERSE = 4;//��]
    private const int INPUT_JUMP = 5;
    private int NowInput = INPUT_NONE;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Manager");
        piller = manager.GetComponent<PillerManager>();
        turnpiller = manager.GetComponent<TurnPillerManager>();


        field = this.GetComponent<Field>();
        field.DefoMoveFlag = true;
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

    private void OnTriggerEnter(Collider collider)
    {
        field.SetNoMove();
        Debug.Log("��������");
        if(collider.gameObject.tag == "Magma")
        {
            Debug.Log("�{�{�{");
        }
    }

    //���͏���
    private void ProcesInput()
    {
        if (NowInput == INPUT_NONE)
        {
            field.SetNoMove();

            if (Input.GetButton("Right"))//�E
            {
                field.SetMove(-speed);
                NowInput = INPUT_RIGHT;
            }
            else if (Input.GetButton("Left"))//��
            {
                field.SetMove(speed);
                NowInput = INPUT_LEFT;
            }
            //else if (Input.GetButtonDown("Reverce"))//��]
            //{
            //    SetReverse();
            //}
            //else if (Input.GetButtonDown("Jump"))//�u���b�N���
            //{
            //    SetBlockUp();
            //}
            //else if (Input.GetKey(KeyCode.P))//�f�o�b�O�p�������ŉ�]����������
            //{
            //    SetReverse();
            //}
        }
        else
        {
            if ((NowInput == INPUT_LEFT && !Input.GetButton("Left")) ||
                (NowInput == INPUT_RIGHT && !Input.GetButton("Right"))
                )
            {
                NowInput = INPUT_NONE;
            }
        }
    }

    //��]�����Z�b�g
    private void SetReverse()
    {
        turnpiller.ReverseStart(this.GetComponent<Field>().nowPiller, false);
        NowInput = INPUT_REVERSE;
    }

    //�u���b�N����Z�b�g
    private void SetBlockUp()
    {
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
        NowInput = INPUT_JUMP;
    }

    private void UpDownMove()
    {
        //��]���͓o��~�肵�Ȃ�
        if (turnpiller.StateReverce())
        {
            return;
        }

        if (BlockUpDownFlag == BLOCK_NONE && field.nowHeight > 0)//������0�ȏ�̎�
        {
            BlockUpDownFlag = BLOCK_DOWN;
            NowInput = INPUT_NONE;
        }

        if (BlockUpDownFlag == BLOCK_UP)//�o�鏈��
        {
            BlockUpMove();
        }
        else if (BlockUpDownFlag == BLOCK_DOWN)
        {
            BlockDownMove();
        }
        
    }

    //�u���b�N�o�鏈��
    private void BlockUpMove()
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
            NowInput = INPUT_NONE;
            MoveFlag = MOVE_NONE;
            BlockUpDownFlag = BLOCK_NONE;
        }
    }


    //��������
    private void BlockDownMove()
    {
        if (MoveFlag == MOVE_NONE)//�����Ɉړ���
        {
            if (field.nowHeight - 1 < 0)//0��菬�����ꍇ
            {
                MoveFlag = MOVE_NONE;
                BlockUpDownFlag = BLOCK_NONE;
                NowInput = INPUT_NONE;
            }
            else//�����J�n
            {
                MoveFlag = MOVE_DOWN;
                NowInput = INPUT_SET;
            }
        }
        else if (MoveFlag == MOVE_DOWN && this.GetComponent<Rigidbody>().isKinematic)//�󒆂ɂ��鎞
        {
            //���n������
            MoveFlag = MOVE_NONE;
            BlockUpDownFlag = BLOCK_NONE;
            NowInput = INPUT_NONE;
        }
    }

    
}
