using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPillerManager : MonoBehaviour
{
    public GameObject PillerPrefub;
    public int ReturnFlame;//���̉�]���ԂP�t���[��1/60

    [HideInInspector] public GameObject[] Piller;//���_���I�u�W�F�N�g
    [HideInInspector] public int[] PillerWidht;//��]����

    private PillerManager FieldPiller;

    private int ReturnPillerID;//�t���O
    private Quaternion ReturnMove;//�P�t���[���̉�]�p�x
    private Vector3 Axis;

    private float flamecount;

    // Start is called before the first frame update
    void Start()
    {
        FieldPiller = this.GetComponent<PillerManager>();

        ReturnPillerID = -1;
        ReturnMove = Quaternion.identity;
        flamecount = 0;
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
    //Pillerid ��ID
    //rotedirection�@��]�����@true ���@false ��O
    public void ReverseStart(int Pillerid, bool rotedirection)
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
        if (rotedirection == false)
        {
            angle *= -1;
        }
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
                //�����t���܂ɂȂ����̂���]�O�ɖ߂�
                Piller[ReturnPillerID].transform.Rotate(Axis, 180.0f);

                //�u���b�N�t���O�֌W
                foreach (Transform child in Piller[ReturnPillerID].transform)
                {
                    Field block = child.GetComponent<Field>();
                    block.FallFlag = true;
                    block.ChangeHeight();
                }

                flamecount = 0;//�t���[���J�E���g���Z�b�g
                ReturnPillerID = -1;//��]���钌�����Z�b�g
            }
        }
    }


    //��]���z��쐬
    //sizenum �v�f��
    public void PrePiller(int sizenum)
    {
        Piller = new GameObject[sizenum];
    }


    public bool StateReverce()
    {
        if (ReturnPillerID != -1)//��]���̏ꍇ
        {
            return true;
        }
        return false;
    }
}
