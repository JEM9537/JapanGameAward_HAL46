using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//�ړ����x
    private PillerManager piller;//�����

    //�t�B�[���h�擾
    Field field;

    private bool InputState;//�ړ����

    private const int BLOCK_NONE = 0;
    private const int BLOCK_UP = 1;
    private const int BLOCK_DOWN = 2;
    private int BlockUpDownFlag;//�u���b�N��艺��t���O

    
    private int MoveFlag;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Manager");
        piller = manager.GetComponent<PillerManager>();


        field = this.GetComponent<Field>();

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

        //���E�ړ���������
        MoveRest();

        //���E�ړ�
        field.ProcesMove();

        //���ύX����
        ProcesPiller();
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
                piller.ReverseStart(this.GetComponent<Field>().nowPiller);
            }
            else if (Input.GetButtonDown("Jump"))//�u���b�N���
            {
                BlockUp();
            }
        }
    }

    //������
    private void ProcesPiller()
    {
        //������
        float pillerposi = ProcessPillerPosi();
        if (Mathf.Abs(field.nowmoveaxis) > pillerposi)//������
        {

            //�ϐ��̒��g�ύX
            if (field.nowmoveaxis <= 0)//�E�ɂ���
            {
                field.nowmoveaxis = pillerposi + (field.nowmoveaxis + pillerposi);//�ړ������Z�b�g
                field.nowPiller = MovePillerID(true);//���݂̒��ύX
            }
            else if (field.nowmoveaxis > 0)//���ɂ���
            {
                field.nowmoveaxis = -pillerposi + (field.nowmoveaxis - pillerposi);//�ړ������Z�b�g
                field.nowPiller = MovePillerID(false);//���݂̒�
            }

            //���ύX
            this.transform.parent = piller.Piller[field.nowPiller].transform;
        }
    }


    private float ProcessPillerPosi()
    {
        return ((360.0f / piller.Aroundnum) / 2.0f);
    }

    //�ړ���~����
    private void MoveRest()
    {
        float nextmove = field.nowmoveaxis + field.moveaxis;//���̈ړ��p�x���v�Z
        if (Mathf.Abs(nextmove) + 0.8f > ProcessPillerPosi())
        {
            if ((field.nowmoveaxis <= 0 && piller.GetPillerBlock(MovePillerID(true), field.nowHeight)) || //�E�ɂ���
                (field.nowmoveaxis > 0 && piller.GetPillerBlock(MovePillerID(false), field.nowHeight)))   //���ɂ���
            {
                //�ړ����Ȃ�
                field.SetNoMove();
            }
        }
    }

    //�u���b�N����
    private void BlockUp()
    {
        BlockUpDownFlag = BLOCK_UP;
    }

    private void UpDownMove()
    {
        if (BlockUpDownFlag == BLOCK_UP)
        {
            //field.SelectChangeHeight(field.nowHeight + 1);
        }
        else if (BlockUpDownFlag == BLOCK_DOWN)
        {
            //field.SelectChangeHeight(field.nowHeight - 1);
        }
        
    }

    //�u���b�N�~���
    private void BlockDown()
    {

    }

    //���̈ړ�ID
    //true �E�@false ��
    private int MovePillerID(bool way)
    {
        if (way)//�E
        {
            return (field.nowPiller + (piller.Aroundnum + 1)) % piller.Aroundnum;
        }

        //��
        return (field.nowPiller + (piller.Aroundnum - 1)) % piller.Aroundnum;
    }
}
