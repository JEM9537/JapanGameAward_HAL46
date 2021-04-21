using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillerManager : MonoBehaviour
{

    public int Aroundnum;//����̒��̐�
    public int ReturnFlame;//���̉�]���ԂP�t���[��1/60

    [HideInInspector]
    public GameObject[] Piller;//���I�u�W�F�N�g


    private int ReturnPillerID;//�t���O
    private Quaternion ReturnMove;//�P�t���[���̉�]�p�x
    private Vector3 Axis;

    private float flamecount;

    private void Awake()
    {
        ReturnPillerID = -1;
        ReturnMove = Quaternion.identity;
        flamecount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //�t���[���Œ�̍X�V
    private void FixedUpdate()
    {
        Reverse();
    }


    //�\�����]
    public void ReverseStart(int Pillerid)
    {
        //��]��
        //���S(�����ƍ�������)�܂ł̕����x�N�g��
        Axis = new Vector3(0.0f, Piller[Pillerid].transform.position.y, 0.0f) - Piller[Pillerid].transform.position;

        //��]�������߂�
        Axis = Vector3.Cross(Axis, Vector3.up);
        Axis.Normalize();//�Ȃ�ƂȂ����K��

        //�t���O�֌W
        foreach (Transform child in Piller[Pillerid].transform)
        {
            Field block = child.GetComponent<Field>();
            block.FallFlag = false;
        }

        //��]����
        float angle = 180.0f / (float)ReturnFlame;
        ReturnMove = Quaternion.AngleAxis(angle, Axis);//���̉�]��

        ReturnPillerID = Pillerid;
    }

    //��]����
    private void Reverse()
    {
        if (ReturnPillerID != -1)//��]���Ă���ꍇ
        {

            //��]
            Piller[ReturnPillerID].transform.localRotation *= ReturnMove;

            //�t���[���J�E���g
            flamecount++;

            if (flamecount >= ReturnFlame)//�t���[���J�E���g���w��̐��l�𒴂�����
            {
                //�u���b�N�t���O�֌W
                foreach (Transform child in Piller[ReturnPillerID].transform)
                {
                    Field block = child.GetComponent<Field>();
                    block.FallFlag = true;
                    block.ChangeHeight();
                }

                //�����t���܂ɂȂ����̂���]�O�ɖ߂�
                Piller[ReturnPillerID].transform.Rotate(Axis, 180.0f);

                flamecount = 0;//�t���[���J�E���g���Z�b�g
                ReturnPillerID = -1;//��]���钌�����Z�b�g
            }

        }
    }

    //���̔z�����������
    public void PrePiller()
    {
        if (Piller.Length <= 0)//�z�񂪐ݒ肳��Ă��Ȃ��ꍇ
        {
            Piller = new GameObject[Aroundnum];
        }
    }

    public bool StateReverce()
    {
        if (ReturnPillerID != -1)//��]���̏ꍇ
        {
            return true;
        }
        return false;
    }

    public bool GetPillerBlock(int piller, int height)
    {
        
        foreach (Transform child in Piller[piller].transform)//���ɒu���Ă���u���b�N������
        {
            if (child.name == ("Block" + height))//�����u���b�N�����݂����ꍇ
            {
                return true;
            }
        }
        return false;
    }

}
