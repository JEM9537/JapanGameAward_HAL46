using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private int m_Frame; // �t���[����
    [SerializeField]
    private int m_DelayTime; // �҂�����

    // Start is called before the first frame update
    void Start()
    {
        m_Frame = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        m_Frame++;
        // �f�B���C�^�C���𒴂����瓮���o��
        if (m_Frame > m_DelayTime)
        {
            Move();
        }
    }

    // �㏸
    private void Move()
    {
        Transform myTransform = this.transform;
        // ���W���擾
        Vector3 pos = myTransform.position;
        pos.y += 0.01f;    // x���W��0.01���Z
        myTransform.position = pos;  // ���W��ݒ�
    }
}
