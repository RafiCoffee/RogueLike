using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Vector2 movement;
    Vector2 movementInput;
    public float movementSpeed = 30f;
    public float jumpForce = 8f;

    public bool canMove;
    private bool isOnGround = true;
    private bool isOnEdge = false;
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
                movementSpeed = 40f;
            }
            else
            {
                movementSpeed = 30f;
            }

            movement = movementInput * movementSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) & isOnGround || Input.GetKeyDown(KeyCode.Space) & isOnEdge)
            {
                playerRb2D.bodyType = RigidbodyType2D.Dynamic;
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

    private void FixedUpdate()
    {
        playerRb2D.transform.Translate(movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.position.y - 0.5f > collision.transform.position.y & collision.GetContact(0).normal.y == 1 & collision.collider.gameObject.layer == 3)
        {
            isOnGround = true;
            isOnEdge = false;
            goodJump = true;
        }

        if (collision.collider.gameObject.layer == 6)
        {
            movement = new Vector2(0, 0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            isOnGround = false;
        }

        if (collision.collider.gameObject.layer == 6)
        {
            movement = movementInput * movementSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 & isOnEdge == false)
        {
            playerRb2D.bodyType = RigidbodyType2D.Static;
            playerRb2D.velocity = new Vector2(0, 0);
            playerRb2D.angularVelocity = 0;
            movement = new Vector2(0, 0);
            canMove = false;
            isOnEdge = true;
        }
    }
}
