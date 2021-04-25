using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int nowHeight { set; get; }

    public int nowPiller { get; set; }

    public int nextPiller { get; set; }

    public bool FallFlag { get; set; }//�����t���O

    //���}�l�[�W���[
    private PillerManager piller;

    //�t���[���J�E���g
    private int FlameCount;
    private float MoveFlame;//�ړ��t���[��
    
    //�ړ��֌W
    private bool YMoveFlag { get; set; }//�ړ��t���O true �ړ����@false �ړ����ĂȂ�
    private float EndPosi;//�ړ���
    private float MoveSpeed;//�ړ����x

    //��]�ړ�
    private bool XMoveFlag { get; set; }
    public Quaternion RotaMove { get; set; }
    public float moveaxis { get; set; }

    public float nowmoveaxis { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Manager");
        piller = manager.GetComponent<PillerManager>();
        SetNoMove();
    }

    // Update is called once per frame
    void Update()
    {
        this.Fall();

        if (Input.GetKeyDown(KeyCode.K))
        {
            SetYMove(nowHeight + 1, 10);
        }
    }

    private void FixedUpdate()
    {
        JumpMove();
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

    private void JumpMove()
    {
        if (YMoveFlag == true)//�ړ�����Ƃ�
        {
            if (FlameCount >= MoveFlame)
            {
                //�t���O
                YMoveFlag = false;
                FallFlag = true;
                nowHeight++;
                this.name = "Block" + nowHeight;
            }

            this.transform.localPosition += new Vector3(0.0f, MoveSpeed, 0.0f);

            FlameCount++;
        }
        else if (XMoveFlag == true)
        {

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

    //�ݒ肳��Ă���I�u�W�F�N�g�𒼐��ړ�����
    //�߂�l�@true �ړ����@false �ړ����ĂȂ�
    //endposi �ړ���
    //flame �ړ��t���[��
    public bool SetYMove(float endposiY, int moveflame)
    {
        FlameCount = 0;
        YMoveFlag = true;
        FallFlag = false;
        MoveFlame = moveflame;
        EndPosi = endposiY;
        MoveSpeed = (EndPosi - nowHeight) / (float)moveflame;
        return YMoveFlag;
    }

    public bool SetAutoMove(int movepillerid, int moveflame)
    {
        FlameCount = 0;
        XMoveFlag = true;
        FallFlag = false;
        MoveFlame = moveflame;

        Vector3 endangle = Create.CalQuaternion(movepillerid, piller.Aroundnum).eulerAngles;//�I�C���[���炤
        Vector3 thisangle = this.transform.rotation.eulerAngles;

        float SAngle = (endangle.y - thisangle.y) / moveflame;//�ړ��p�x�v�Z

        RotaMove = Quaternion.AngleAxis(SAngle, Vector3.up);

        return XMoveFlag;
    }

    //�u���b�N���{������͂��̍��W��Ԃ�
    private Vector3 MovePosition()
    {
        return new UnityEngine.Vector3(
            this.transform.localPosition.x, 
            (float)nowHeight - this.transform.parent.localPosition.y, 
            this.transform.localPosition.z);
    }

    //�ړ�����
    public void ProcesMove()
    {
        this.transform.position = RotaMove * this.transform.position;
        nowmoveaxis += moveaxis;
    }
    
    //�ړ��o�^
    //angle �ړ��p�x
    public void SetMove(float angle)
    {
        RotaMove = Quaternion.AngleAxis(angle, Vector3.up);
        moveaxis = angle;
    }

    //�ړ����Ȃ�����
    public void SetNoMove()
    {
        RotaMove = Quaternion.identity;
        moveaxis = 0.0f;
    }
    
}
