using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject FlameObject;//�t���[���I�u�W�F�N�g

    public GameObject Floor { get; set; }//���݂̃t���A�I�u�W�F�N�g
    public int Floornum { get; set; } //�t���A��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��̊K�w�ɐi��
    public void UpFloor()
    {
        Floornum++;
    }
}
