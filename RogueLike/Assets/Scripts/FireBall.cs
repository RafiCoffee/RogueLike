using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private CircleCollider2D fireBallCc;
    // Start is called before the first frame update
    void Start()
    {
        fireBallCc = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
