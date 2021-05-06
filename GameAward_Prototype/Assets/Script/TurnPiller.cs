using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPiller : MonoBehaviour
{
    //��]���x
    public int ReturnFlame;

    public int size { get; set; }

    private Field field;//�t�B�[���h

    //���}�l�[�W���[
    private PillerManager piller;

    private bool ReturnFlag;
    private Quaternion Move;
    private Vector3 Axis;

    private int flamecount;


    // Start is called before the first frame update
    void Start()
    {
        ReturnFlag = false;
        Move = Quaternion.identity;
        Axis = Vector3.zero;
        flamecount = 0;

        field = this.GetComponent<Field>();

        GameObject manager = GameObject.Find("Manager");
        piller = manager.GetComponent<PillerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ReverseStart(true);
        }
    }

    private void FixedUpdate()
    {
        Reverse();
    }

    //�\�����]
    //Pillerid ��ID
    //rotedirection�@��]�����@true ���@false ��O
    public void ReverseStart(bool rotedirection)
    {
        //��]��
        //���S(�����ƍ�������)�܂ł̕����x�N�g��
        Axis = new Vector3(0.0f, this.transform.position.y, 0.0f) - this.transform.position;

        //��]�������߂�
        Axis = Vector3.Cross(Axis, Vector3.up);
        Axis.Normalize();//�Ȃ�ƂȂ����K��

        //�u���b�N��񓙂��󂯎��
        for (int i = 0; i < size * 2; i++)
        {
            GameObject obj = piller.GetBlockObject(field.nowPiller, field.nowHeight + size - 1 - i);
            if (obj != null)
            {
                obj.transform.parent = this.transform;
            }
        }

        //�t���O�֌W
        foreach (Transform child in this.transform)
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
        Move = Quaternion.AngleAxis(angle, Axis);//���̉�]��

        ReturnFlag = true;
    }

    //��]����
    private void Reverse()
    {
        if (ReturnFlag == true)//��]���Ă���ꍇ
        {

            //��]
            this.transform.rotation *= Move;

            //�t���[���J�E���g
            flamecount++;

            if (flamecount >= ReturnFlame)//�t���[���J�E���g���w��̐��l�𒴂�����
            {

                //�����t���܂ɂȂ����̂���]�O�ɖ߂�
                this.transform.Rotate(Axis, 180.0f);

                //�u���b�N�t���O�֌W
                foreach (Transform child in this.transform)
                {
                    Field block = child.GetComponent<Field>();
                    block.FallFlag = true;
                    block.ChangeHeight(field.nowHeight);
                    child.transform.parent = this.transform.parent;
                    child.transform.position = new Vector3(child.transform.position.x, block.nowHeight, child.transform.position.z);
                }

                flamecount = 0;//�t���[���J�E���g���Z�b�g
                ReturnFlag = false;//��]���钌�����Z�b�g
            }
        }
    }
}
