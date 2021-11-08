using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2DVC : MonoBehaviour
{
    [SerializeField, Range(0, 10)] 
    public int vida = 10;

    Vector2 movement;
    Vector2 movementInput;
    Vector2 colision;
    public float movementSpeed = 30f;

    public bool canMove;

    private Rigidbody2D playerRb2D;
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 40f;
            }
            else
            {
                movementSpeed = 30f;
            }

            movement = movementInput.normalized * movementSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerRb2D.AddForce(Vector2.up * 20000, ForceMode2D.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        playerRb2D.MovePosition(playerRb2D.position + movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            colision = collision.GetContact(0).normal;
            Debug.Log(colision);
        }
    }
}
