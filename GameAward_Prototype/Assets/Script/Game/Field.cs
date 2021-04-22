using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int nowHeight { set; get; }


    public int nowPiller { get; set; }

    public bool FallFlag { get; set; }//�����t���O

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.Fall();
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

    //�u���b�N���{������͂��̍��W��Ԃ�
    private Vector3 MovePosition()
    {
        return new Vector3(
            this.transform.localPosition.x, 
            (float)nowHeight - this.transform.parent.localPosition.y, 
            this.transform.localPosition.z);
    }
}
