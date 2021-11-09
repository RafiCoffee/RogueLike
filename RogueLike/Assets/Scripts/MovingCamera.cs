using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private GameObject mainCamera;

    private Transform pointCamera;

    private int children;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        pointCamera = GameObject.FindGameObjectWithTag("CameraPoint").transform;

        mainCamera.transform.position = pointCamera.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("EntryRoom") || collision.CompareTag("NormalRoom"))
        {
            children = collision.transform.childCount;
            pointCamera = collision.transform.GetChild(children - 1);
            mainCamera.transform.position = pointCamera.position;
        }
    }
}
