using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;//�ړ����x
    public PillerManager piller;//�����

    //�t�B�[���h�擾
    Field field;

    public Quaternion move { get; set; }//�ړ�
    private float moveaxis;

    private float nowmove;//���̊p�x
    
    // Start is called before the first frame update
    void Start()
    {
        move = Quaternion.identity;
        nowmove = 0.0f;
        field = this.GetComponent<Field>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        //���͏���
        ProcesInput();

        //�ړ���������
        MoveRest();

        //�ړ�
        ProcesMove();

        //������
        ProcesPiller();
    }

    //���͏���
    private void ProcesInput()
    {
        if (!piller.StateReverce())
        {
            SetNoMove();

            if (Input.GetButton("Right"))//�E
            {
                SetMove(-speed);
            }
            else if (Input.GetButton("Left"))//��
            {
                SetMove(speed);
            }
            else if (Input.GetButton("Reverce"))//��]
            {
                piller.ReverseStart(this.GetComponent<Field>().nowPiller);
            }
        }
    }

    //������
    private void ProcesPiller()
    {
        //������
        float pillerposi = ProcessPillerPosi();
        if (Mathf.Abs(nowmove) > pillerposi)//������
        {
            //�t�B�[���h�擾
            Field field = this.GetComponent<Field>();

            //�ϐ��̒��g�ύX
            if (nowmove <= 0)//�E�ɂ���
            {
                nowmove = pillerposi + (nowmove + pillerposi);//�ړ������Z�b�g
                field.nowPiller = MovePillerID(true);//���݂̒��ύX
            }
            else if (nowmove > 0)//���ɂ���
            {
                nowmove = -pillerposi + (nowmove - pillerposi);//�ړ������Z�b�g
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
        float nextmove = nowmove + moveaxis;//���̈ړ��p�x���v�Z
        if (Mathf.Abs(nextmove) + 0.8f > ProcessPillerPosi())
        {
            if ((nowmove <= 0 && piller.GetPillerBlock(MovePillerID(true), field.nowHeight)) || //�E�ɂ���
                (nowmove > 0 && piller.GetPillerBlock(MovePillerID(false), field.nowHeight)))   //���ɂ���
            {
                SetNoMove();//�ړ����Ȃ�
            }
        }
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

    //�ړ�
    private void ProcesMove()
    {
        this.transform.position = move * this.transform.position;
        nowmove += moveaxis;
    }

    //�ړ��\��
    private void SetMove(float angle)
    {
        move = Quaternion.AngleAxis(angle, Vector3.up);
        //nowmove += angle;
        moveaxis += angle;
    }

    //�ړ����Ȃ�
    private void SetNoMove()
    {
        move = Quaternion.identity;
        moveaxis = 0.0f;
    }
}
