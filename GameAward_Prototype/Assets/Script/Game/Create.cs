using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    FloorManager floor;//�K�w
    PillerManager piller;//��
    BlockManager block;//�u���b�N
    PlayerManager player;//�v���C���[


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


        SetFloorData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(int Pilleerid, int height)
    {
        //�ړ���
        Quaternion move = CalQuaternion(Pilleerid, piller.Aroundnum);

        //���W�v�Z
        Vector3 posi = CalPosition(move, DefaultPosition, height);
        Quaternion rotation = CalRotation(move, player.Player.transform.rotation);

        //�Z�b�g
        player.Player.transform.parent = piller.Piller[Pilleerid].transform;
        player.Player.transform.position = posi;
        player.Player.transform.localPosition = new Vector3(0.0f, -2.0f, 0.0f);
        player.Player.transform.localRotation = rotation;
        Field field = player.Player.GetComponent<Field>();
        field.FallFlag = true;
        field.nowHeight = height;
        field.nowPiller = Pilleerid;
    }

    //�K�w��ݒ�
    public void SetFloorData()
    {
        //�V�����K�w���쐬
        floor.Floor = CreateObject("Floor" + floor.Floornum);

        //�V�����t���[���쐬
        GameObject flame = Instantiate(floor.FlameObject);
        flame.transform.parent = floor.Floor.transform;

        //�V���������쐬
        SetPiller();

        //�V�����u���b�N���쐬
        SetBlock(1, 3);
        SetBlock(1, 1);
        SetBlock(2, 0);
        SetBlock(2, 2);

        //��̊K�w�ɂ�����
        floor.UpFloor();
    }

    //����ݒ�
    private void SetPiller()
    {
        //���z��쐬
        piller.PrePiller();

        //�����ݒ�
        for (int i = 0; i < piller.Aroundnum; i++)
        {
            piller.Piller[i] = CreateObject("Piller" + i);
            piller.Piller[i].transform.parent = floor.Floor.transform;

            Quaternion move = CalQuaternion(i, piller.Aroundnum);
            piller.Piller[i].transform.position = CalPosition(move, DefaultPosition, 2);
        }
    }

    //�u���b�N�ݒ�
    //Pillernum �u���b�N�𐶐�����u���b�N�̒��̈ʒu
    //height �u���b�N�̍����@�O�X�^�[�g
    private void SetBlock(int Pillerid, int height)
    {
        //��]�v�Z
        Quaternion move = CalQuaternion(Pillerid, piller.Aroundnum);

        //Transform�f�[�^
        Vector3 posi = CalPosition(move, DefaultPosition, height);
        Quaternion rotation = CalRotation(move, block.BlockObject.transform.rotation);

        //�u���b�N�쐬
        GameObject newblock = Instantiate(block.BlockObject);
        newblock.transform.parent = piller.Piller[Pillerid].transform;//�e�ݒ�
        newblock.transform.position = posi;//���W�ݒ�
        newblock.transform.localPosition = new Vector3(0.0f, -2f, 0.0f);
        newblock.transform.rotation = rotation;//��]�ݒ�
        newblock.name = "Block" + height;//���O�ݒ�
        Field field = newblock.GetComponent<Field>();
        field.nowHeight = height;//����
        //field.nowPiller = piller.Piller[Pillerid];//�����̂��钌�o�^
        field.nowPiller = Pillerid;
        //field.nextPiller = field.nowPiller;
        field.FallFlag = true;
    }

    //��̃I�u�W�F�N�g�쐬
    private GameObject CreateObject(string name)
    {
        return new GameObject(name);
    }

    //�ړ����钌�̊p�x�v�Z
    //Pillernum�͒��ԍ��@0�ɂ���΍��̈ʒu�̂܂�
    static public Quaternion CalQuaternion(int Pillerid, int Aroundnum)
    {
        //������̊p�x
        float OnePiller = 360.0f / (float)Aroundnum;
        Quaternion quaternion = Quaternion.Euler(0.0f, -OnePiller * (float)Pillerid, 0.0f);

        return quaternion;
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
}
