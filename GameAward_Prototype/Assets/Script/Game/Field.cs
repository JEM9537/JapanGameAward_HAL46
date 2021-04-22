using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int nowHeight { set; get; }


    public int nowPiller { get; set; }

    public bool FallFlag { get; set; }//�����t���O

    //�ړ��֌W
    private int FlameCount;//�t���[���J�E���g
    private float MoveFlame;//�ړ��t���[��
    private bool MoveFlag;//�ړ��t���O true �ړ����@false �ړ����ĂȂ�
    private float EndPosi;//�ړ���
    private float MoveSpeed;//�ړ����x

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.Fall();

        if (Input.GetKeyDown(KeyCode.K))
        {
            SetLineMove(nowHeight + 1, 10);
        }
    }

    private void FixedUpdate()
    {
        LineMovePosition();
    }

    //��������
    private void Fall()
    {
        if (!FallFlag)
        {
            return;
        }

        //���̃��[�J�����W��Y��+2����Ă��邽�ߕ␳����
        if (this.transform.localPosition.y + this.transform.parent.localPosition.y <= (float)nowHeight)
        {

            //���W�Œ�
            this.transform.localPosition = MovePosition();

            //rigitbody�̉e�����󂯂Ȃ�����
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            //rigitbody�̉e�����󂯂�悤�ɂ���
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void LineMovePosition()
    {
        if (MoveFlag == true)//�ړ�����Ƃ�
        {
            if (FlameCount >= MoveFlame)
            {
                //�t���O
                MoveFlag = false;
                FallFlag = true;
                nowHeight++;
            }

            this.transform.localPosition += new Vector3(0.0f, MoveSpeed, 0.0f);

            FlameCount++;
        }
    }

    //������ύX����
    public void ChangeHeight()
    {
        if (nowHeight == 0)//��ԉ������ԏ�
        {
            nowHeight += 3;
        }
        else if (nowHeight == 1)//�^�񒆉������
        {
            nowHeight += 1;
        }
        else if (nowHeight == 2)//�^�񒆏ォ�牺
        {
            nowHeight -= 1;
        }
        else if (nowHeight == 3)//��ԏォ���ԉ�
        {
            nowHeight -= 3;
        }

        this.name = "Block" + nowHeight;//���O����������
        this.transform.localPosition = MovePosition();//���W��ύX
    }


    //�w�肵�������ɕύX
    //height  �ύX���鍂��
    public int SelectChangeHeight(int height)
    {
        int prevheight = nowHeight;//�O�̍�����ۑ�
        nowHeight = height;//�ύX���鍂����ύX

        return prevheight;
    }

    //�ݒ肳��Ă���I�u�W�F�N�g�𒼐��ړ�����
    //�߂�l�@true �ړ����@false �ړ����ĂȂ�
    //endposi �ړ���
    //flame �ړ��t���[��
    public bool SetLineMove(float endposiY, int moveflame)
    {
        if (MoveFlag == false)
        {
            FlameCount = 0;
            MoveFlag = true;
            FallFlag = false;
            MoveFlame = moveflame;
            EndPosi = endposiY;
            MoveSpeed = (EndPosi - nowHeight) / (float)moveflame;
            
        }
        return MoveFlag;
    }

    //�u���b�N���{������͂��̍��W��Ԃ�
    private Vector3 MovePosition()
    {
        return new UnityEngine.Vector3(
            this.transform.localPosition.x, 
            (float)nowHeight - this.transform.parent.localPosition.y, 
            this.transform.localPosition.z);
    }

    

    
    
}
