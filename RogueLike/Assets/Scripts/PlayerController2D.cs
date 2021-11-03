using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Vector2 movement;
    Vector2 movementInput;
    public float movementSpeed = 10f;
    public float jumpForce = 20f;

    public bool canMove;
    private bool isOnGround = true;
    private bool goodJump = true;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D playerBc2D;
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerBc2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movementInput.x = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 10f;
            }
            else
            {
                movementSpeed = 5f;
            }

            movement = movementInput * movementSpeed * Time.deltaTime;

            transform.Translate(movement);

            if (Input.GetKeyDown(KeyCode.Space) & isOnGround)
            {
                playerRb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isOnGround = false;
                goodJump = true;
            }

            if (Input.GetKeyUp(KeyCode.Space) & isOnGround == false & goodJump)
            {
                goodJump = false;
                do
                {
                    playerRb2D.velocity = new Vector2(playerRb2D.velocity.x, playerRb2D.velocity.y - 2);
                }
                while (playerRb2D.velocity.y == 0);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            isOnGround = true;
            goodJump = true;
        }

        if (collision.collider.gameObject.layer == 6)
        {
            movement = new Vector2(0, 0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 6)
        {
            movement = movementInput * movementSpeed * Time.deltaTime;
        }
    }
}
