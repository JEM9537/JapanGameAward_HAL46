using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float Length;//����


    private GameObject player;//�v���C���[
    private PillerManager piller;//�����

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[�I�u�W�F�N�g�Ⴄ
        player = GameObject.Find("Player").gameObject;

        //�����
        piller = GameObject.Find("Manager").gameObject.GetComponent<PillerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!piller.StateReverce())
        {
            //�v���C���[�̏����J�����p�ɉ��H�������
            Vector3 zikuposi = new Vector3(player.transform.position.x, 2.0f, player.transform.position.z);

            //�
            Vector3 kijun = new Vector3(0.0f, 2.0f, 0.0f);

            //�x�N�g��
            Vector3 vec = zikuposi - kijun;
            Vector3.Normalize(vec);//���K��

            //���W�ݒ�
            this.transform.position = kijun + (vec * Length);

            //���|�W������
            this.transform.LookAt(zikuposi);
        }

        
    }
}
