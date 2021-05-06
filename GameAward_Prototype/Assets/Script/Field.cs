using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int nowHeight; //{ set; get; }

    public int nowPiller;//{ get; set; }

    public int nextPiller;// { get; set; }

    public bool FallFlag;//{ get; set; }//�����t���O

    //���}�l�[�W���[
    private PillerManager piller;

    //�t���[���J�E���g
    private int FlameCount;
    private float MoveFlame;//�ړ��t���[��
    
    //�ړ��֌W
    public bool YMoveFlag { get; set; }//�ړ��t���O true �ړ����@false �ړ����ĂȂ�
    private float YEndPosi;//�ړ���
    private float YMoveSpeed;//�ړ����x

    public bool XMoveFlag { get; set; }

    //�����Ɉړ�
    public bool MoveCenter { get; set; }

    //��]�ړ�
    public bool DefoMoveFlag { get; set; }
    public Quaternion RotaMove { get; set; }
    public float moveaxis { get; set; }

    public float nowmoveaxis { get; set; }

    private void Awake()
    {
        DefoMoveFlag = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        YMoveFlag = false;
        YEndPosi = 0.0f;
        YMoveSpeed = 0.0f;

        XMoveFlag = false;
        MoveCenter = false;

        GameObject manager = GameObject.Find("Manager");
        piller = manager.GetComponent<PillerManager>();
        SetNoMove();

        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.Fall();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //�����蔻��
    void OnCollisionEnter(Collision collision)
    {
        if (FallFlag)
        {
            //rigitbody�̉e�����󂯂Ȃ�����
            this.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    //��������
    private void Fall()
    {
        if (!FallFlag)
        {
            return;
        }

        //�O�ȉ��ɂȂ�Ȃ��悤�ɂ���
        if (this.transform.position.y <= 0.0f)
        {
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (this.transform.position.y < nowHeight)//���݂̍�����菬�����ꍇ
        {
            nowHeight--;
        }
    }

    

    private void Move()
    {
        if (YMoveFlag == true)//�ړ�����Ƃ�
        {
            if (FlameCount >= MoveFlame)
            {
                //�t���O
                YMoveFlag = false;
                FallFlag = true;
                SelectChangeHeight(nowHeight + 1);
            }

            this.transform.localPosition += new Vector3(0.0f, YMoveSpeed, 0.0f);

            FlameCount++;
        }
        else if (XMoveFlag == true)
        {

            if ((nextPiller == nowPiller &&
                ((moveaxis >= 0.0f && !NowWay()) ||//���ړ�
                 (moveaxis <= 0.0f && NowWay()))))//�E�ړ�
            {
                FallFlag = true;
                XMoveFlag = false;
                SetNoMove();
            }
            else
            {
                ProcesMove();
            }

            //���ύX����
            ProcesPiller();
        }
        else if (MoveCenter == true)
        {
            if (((moveaxis >= 0.0f && !NowWay()) ||//���ړ�
                 (moveaxis <= 0.0f && NowWay())))//�E�ړ�
            {
                FallFlag = true;
                MoveCenter = false;
                SetNoMove();
            }
            else
            {
                ProcesMove();
            }

            //���ύX����
            ProcesPiller();
        }
        else if(DefoMoveFlag == true)
        {
            //���E�ړ���������
            MoveRest();

            //���E�ړ�
            ProcesMove();

            //���ύX����
            ProcesPiller();
        }

        
    }

    //������ύX����
    //rotateheight ��]���@����
    public void ChangeHeight(int rotateheight)
    {

        int range = rotateheight - nowHeight;
        nowHeight = rotateheight + range - 1;
        
    }

    public void SelectChangeHeight(int height)
    {
        nowHeight = height;
        if (this.name != "Player")
        {
            this.name = "Block" + nowHeight;//���O����������
        }
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
        YEndPosi = endposiY;
        YMoveSpeed = (YEndPosi - nowHeight) / (float)moveflame;
        return YMoveFlag;
    }

    //�������ړ�
    //way true �E�@false ��
    //moveflame �ړ��t���[����
    public bool SetXMove(bool way, int moveflame)
    {
        XMoveFlag = true;
        FallFlag = false;
        nextPiller = MovePillerID(way);

        float XMoveSpeed = CalFlameMove(moveflame);


        if (way == true)
        {
            XMoveSpeed *= -1;
        }

        SetMove(XMoveSpeed);//�ړ��p�x�Z�b�g

        return XMoveFlag;
    }

    public bool SetCenterMove(int moveflame)
    {
        MoveCenter = true;
        FallFlag = false;
        nextPiller = nowPiller;

        float XMoveSpeed = CalFlameMove(moveflame);

        if (NowWay() == true)
        {
            XMoveSpeed *= -1;
        }

        SetMove(XMoveSpeed);//�ړ��p�x�Z�b�g

        return MoveCenter;
    }

    private float CalFlameMove(float moveflame)
    {
        float oneangle = 360.0f / (float)piller.Aroundnum;//������̈ړ��p�x
        float XMoveSpeed = oneangle / moveflame;//�ړ��p�x�v�Z
        return XMoveSpeed = Mathf.Abs(XMoveSpeed);
    }

    //�u���b�N���{������͂��̍��W��Ԃ�
    private Vector3 MovePosition()
    {
        return new UnityEngine.Vector3(
            this.transform.localPosition.x, 
            (float)nowHeight - this.transform.parent.localPosition.y, 
            this.transform.localPosition.z);
    }

    //�����̈ړ����v�Z
    private void ProcesPiller()
    {
        //������
        float pillerposi = ProcessPillerPosi();
        if (Mathf.Abs(nowmoveaxis) > pillerposi)//������
        {

            //�ϐ��̒��g�ύX
            if (nowmoveaxis <= 0)//�E�ɂ���
            {
                nowmoveaxis = pillerposi + (nowmoveaxis + pillerposi);//�ړ������Z�b�g
                nowPiller = MovePillerID(true);//���݂̒��ύX
                this.transform.parent = piller.FieldPiller[nowPiller].transform;
            }
            else if (nowmoveaxis > 0)//���ɂ���
            {
                nowmoveaxis = -pillerposi + (nowmoveaxis - pillerposi);//�ړ������Z�b�g
                nowPiller = MovePillerID(false);//���݂̒�
                this.transform.parent = piller.FieldPiller[nowPiller].transform;
            }
        }
    }

    //�ړ���~����
    private bool MoveRest()
    {
        float nextmove = nowmoveaxis + moveaxis;//���̈ړ��p�x���v�Z
        if (Mathf.Abs(nextmove) + 3.0f > ProcessPillerPosi())
        {
            if ((NowWay() && piller.GetPillerBlock(MovePillerID(true), nowHeight)) || //�E�ɂ���
                (!NowWay() && piller.GetPillerBlock(MovePillerID(false), nowHeight)))   //���ɂ���
            {
                //�ړ����Ȃ�
                SetNoMove();

                return false;
            }
        }

        return true;
    }

    //���ړ�
    private float ProcessPillerPosi()
    {
        return ((360.0f / piller.Aroundnum) / 2.0f);
    }

    //���̈ړ�ID
    //true �E�@false ��
    public int MovePillerID(bool way)
    {
        if (way)//�E
        {
            return (nowPiller + (piller.Aroundnum + 1)) % piller.Aroundnum;
        }

        //��
        return (nowPiller + (piller.Aroundnum - 1)) % piller.Aroundnum;
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

    public void SetNoDown()
    {
        SetNoMove();
        this.GetComponent<Rigidbody>().isKinematic = false;
        FallFlag = false;
    }

    //���݂̒��̈ʒu
    //true ���̒��S����E���ɂ���
    //false ���̒��S���獶���ɂ���
    public bool NowWay()
    {
        if (nowmoveaxis <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public bool StateReverse()
    {
        if (this.transform.parent.name.Contains("Turn"))
        {
            return true;
        }

        return false;
    }
    
}
