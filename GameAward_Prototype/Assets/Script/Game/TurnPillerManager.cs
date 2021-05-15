using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TurnPillerManager : MonoBehaviour
{
    public GameObject PillerPrefub;
    public int ReturnFlame;//���̉�]���ԂP�t���[��1/60

    [HideInInspector] public GameObject[] Piller;//���_���I�u�W�F�N�g

    private PillerManager FieldPiller;

    // Start is called before the first frame update
    void Start()
    {
        FieldPiller = this.GetComponent<PillerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //�t���[���Œ�̍X�V
    private void FixedUpdate()
    {

    }

    //��]���z��쐬
    //sizenum �v�f��
    public void PrePiller(int sizenum)
    {
        Piller = new GameObject[sizenum];
        for (int i = 0; i < Piller.Length; i++)
        {
            Piller[i] = null;
        }
    }

    //��]������z��ɃZ�b�g
    public void SetPiller(GameObject piller)
    {
        int i = 0;
        for (i = 0; i < Piller.Length; i++)
        {
            if (Piller[i] == null)
            {
                Piller[i] = piller;
                return;
            }
        }

        Array.Resize(ref Piller, Piller.Length + 5);
        Piller[i] = piller;
    }

    //��]�ł��������������]�J�n
    public GameObject StartReverse(int pillerid, int height)
    {
        GameObject obj = null;
        for (int i = 0; i < Piller.Length; i++)
        {
            
            //���g���Ȃ��Ƃ���܂ŗ������]���Ȃ�
            if (Piller[i] == null)
            {
                return obj;
            }

            //�K�v�Ȃ�擾
            Field field = Piller[i].GetComponent<Field>();
            TurnPiller turnPiller = Piller[i].GetComponent<TurnPiller>();

            //�������łȂ������͈̔͂ɂ��邩
            int ereamax = field.nowHeight + turnPiller.size - 1;
            int ereamin = field.nowHeight - turnPiller.size;
            if (field.nowPiller == pillerid &&
                (ereamax >= height && ereamin <= height))
            {
                turnPiller.ReverseStart(true);
                obj = Piller[i];
                return obj;
            }
        }
        return obj;
    }
}
