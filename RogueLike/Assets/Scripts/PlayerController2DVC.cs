using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController2DVC : MonoBehaviour
{
    private NavMeshAgent playerAI;
    Vector2 movement;
    Vector2 movementInput;
    public float movementSpeed = 10f;

    public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        playerAI = GetComponent<NavMeshAgent>();
        playerAI.updateRotation = false;
        playerAI.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movementInput.x = Input.GetAxis("Horizontal");
            movementInput.y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 10f;
            }
            else
            {
                movementSpeed = 5f;
            }

            movement = movementInput * movementSpeed * Time.deltaTime;

            playerAI.Move(movement);
        }
    }
}
