using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 diffVector;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;  //takip edilen nesnenin transformu giriliyor
        diffVector = player.position - transform.position; //offset bulunuyor
    }

    // Update is called once per frame
    void LateUpdate()  //daha sonra çalışacağı için
    {
        transform.position = player.position - diffVector; //sürekli güncelleme yaparak kamera takip ediyor
    }
}
