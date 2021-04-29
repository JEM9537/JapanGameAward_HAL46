using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    FloorManager floor;//�K�w
    PillerManager piller;//��
    BlockManager block;//�u���b�N
    PlayerManager player;//�v���C���[
    TurnPillerManager turnpiller;//��]��


    private const float defoX = 4.45f;
    private const float defoY = 0.0f;
    private const float defoZ = 0.0f;

    Vector3 DefaultPosition = new Vector3(defoX, defoY, defoZ);



    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        floor = this.GetComponent<FloorManager>();
        piller = this.GetComponent<PillerManager>();
        block = this.GetComponent<BlockManager>();
        player = this.GetComponent<PlayerManager>();
        turnpiller = this.GetComponent<TurnPillerManager>();

        //�X�e�[�W���쐬
        SetFieldPiller();

        //�v���C���[�Z�b�g
        SetPlayer(0, 10);

        //�u���b�N�Z�b�g
        SetBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetFieldPiller()
    {
        //�X�e�[�W���쐬
        piller.PreFieldPiller();

        for (int i = 0; i < piller.Aroundnum; i++)
        {
            piller.FieldPiller[i] = CreateObject("FieldPiller" + i);
        }
    }

    //�v���C���[�z�u
    //side �����W
    //height�@�������W
    public void SetPlayer(int side, int height)
    {
        side = CalPillerid(side);

        //�ړ���
        Quaternion move = CalQuaternion(side, piller.Aroundnum);

        //���W�v�Z
        Vector3 posi = CalPosition(move, DefaultPosition, height);
        Quaternion rotation = CalRotation(move, player.Player.transform.rotation);

        //�Z�b�g
        player.Player.transform.parent = piller.FieldPiller[side].gameObject.transform;//�e�ݒ�
        player.Player.transform.position = posi;//���W
        player.Player.transform.rotation = rotation;//��]
        Field field = player.Player.GetComponent<Field>();//�t�B�[���h�󂯎��
        field.FallFlag = true;//�����t���O
        field.nowHeight = height;//����
        field.nowPiller = side;
    }

    //����ݒ�
    private void SetPiller(int pillerid ,int height)
    {
        //�����ݒ�
        turnpiller.Piller[pillerid] = CreateObject("Piller" + pillerid);

        //���W�v�Z
        Quaternion move = CalQuaternion(pillerid, piller.Aroundnum);
        Vector3 posi = CalPosition(move, DefaultPosition, height);

        //�Z�b�g

    }

    private void SetBlock()
    {
        //�u���b�N����
        CreateBlock(1, 0);
    }

    private GameObject CreatePiller(int side, int height)
    {
        side = CalPillerid(side);

        GameObject obj = CreateObject("Turn" + side + "_" + height);


        return obj;
    }

    //�u���b�N�ݒ�
    //side ���ʒu
    //height ����
    private void CreateBlock(int side, int height)
    {
        side = CalPillerid(side);

        //��]�v�Z
        Quaternion move = CalQuaternion(side, piller.Aroundnum);

        //Transform�f�[�^
        Vector3 posi = CalPosition(move, DefaultPosition, height);
        Quaternion rotation = CalRotation(move, block.BlockObject.transform.rotation);

        //�u���b�N�쐬
        GameObject newblock = Instantiate(block.BlockObject);
        newblock.transform.parent = piller.FieldPiller[side].gameObject.transform;
        newblock.transform.position = posi;//���W�ݒ�
        newblock.transform.rotation = rotation;//��]�ݒ�
        newblock.name = "Block" + side + "_" + height;//���O�ݒ�
        Field field = newblock.GetComponent<Field>();
        field.nowHeight = height;//����
        field.FallFlag = false;
        field.nowPiller = side;
    }

    //��̃I�u�W�F�N�g�쐬
    private GameObject CreateObject(string name)
    {
        return new GameObject(name);
    }

    //�ړ����钌�̊p�x�v�Z�@�N�H�[�^�j�I��
    //Pillernum�͒��ԍ��@0�ɂ���΍��̈ʒu�̂܂�
    //AroundNum�@���̐�
    static public Quaternion CalQuaternion(int Pillerid, int Aroundnum)
    {
        //������̊p�x
        float OnePiller = 360.0f / (float)Aroundnum;
        Quaternion quaternion = Quaternion.Euler(CalVector3(Pillerid, Aroundnum));

        return quaternion;
    }

    //�ړ����钌�̊p�x�v�Z �I�C���[�p
    //Pillernum�͒��ԍ��@0�ɂ���΍��̈ʒu�̂܂�
    //AroundNum�@���̐�
    static public Vector3 CalVector3(int Pillerid, int Aroundnum)
    {
        float OnePiller = 360.0f / (float)Aroundnum;
        return new Vector3(0.0f, -OnePiller * (float)Pillerid, 0.0f);
    }

    //���W�v�Z
    //move CalQuaternion�Ōv�Z�����l
    //baceposi ���W�
    //height ����0~3
    private Vector3 CalPosition(Quaternion move, Vector3 baceposi, int height)
    {
        Vector3 newposi = baceposi + new Vector3(0.0f, (float)height, 0.0f);
        return move * newposi;
    }

    //��]�v�Z
    //move CalQuaternion�Ōv�Z�����l
    //BaceRotation ��]�
    private Quaternion CalRotation(Quaternion move, Quaternion BaceRotation)
    {
        return move * BaceRotation;
    }

    private int CalPillerid(int pillerid)
    {
        return pillerid % piller.Aroundnum;
    }
}
