using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoRotate : MonoBehaviour
{
    private Quaternion rotacion;
    // Start is called before the first frame update
    void Start()
    {
        rotacion = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rotacion;
    }
}
