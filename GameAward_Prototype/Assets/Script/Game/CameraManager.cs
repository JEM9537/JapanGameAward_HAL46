using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;//�v���C���[

    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        

        //�����v�Z
        Vector3 posi = new Vector3(player.transform.position.x, 2.0f, player.transform.position.z);
        Vector3 vec = posi - new Vector3(0.0f, 2.0f, 0.0f);
        Vector3.Normalize(vec);//���K��
        this.transform.position = posi + (vec * 1);

        this.transform.LookAt(posi);
    }
}
