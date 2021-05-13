using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillerManager : MonoBehaviour
{

    public int Aroundnum;//����̒��̐�
    [HideInInspector] public GameObject[] FieldPiller;//���Ǘ�

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    //���̔z�����������
    public void PreFieldPiller()
    {
        if (FieldPiller.Length <= 0)//�z�񂪐ݒ肳��Ă��Ȃ��ꍇ
        {
            FieldPiller = new GameObject[Aroundnum];
        }
    }



    public bool GetPillerBlock(int piller, int height)
    {
        GameObject obj = GetObject(piller, height);
        
        if (obj == null || obj.tag != "Block")
        {
            return false;
        }

        return true;
    }

    public GameObject GetObject(int piller, int height)
    {
        foreach (Transform child in FieldPiller[piller].transform)
        {
            Field field = child.GetComponent<Field>();
            
            if (height == field.nowHeight)
            {
                return child.gameObject;
            }
        }
        GameObject obj = null;
        return obj;
    }

}
