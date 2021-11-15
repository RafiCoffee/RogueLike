using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zCameraDepth = -Camera.main.transform.position.z;
        Vector3 MouseScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCameraDepth);
        Vector3 MouseWorldPoint = Camera.main.ScreenToWorldPoint(MouseScreenPoint);
        Vector3 lookAtDirection = MouseWorldPoint - transform.position;

        transform.up = lookAtDirection;
    }
}
