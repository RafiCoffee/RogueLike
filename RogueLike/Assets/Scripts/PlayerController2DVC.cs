using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2DVC : MonoBehaviour
{
    [SerializeField, Range(0, 10)] 
    public int vida = 10;
    [SerializeField, Range(0, 10)]
    public int dashForce = 10;

    Vector2 movement;
    Vector2 movementInput;
    Vector2 colision;
    Vector3 point;
    Vector2 mousePos;

    public float movementSpeed = 30f;

    public bool canMove = false;

    private Rigidbody2D playerRb2D;
    private GameObject attackCollider;

    private RoomTemplates templates;
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        attackCollider = GameObject.Find("AttackCollider");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        attackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (templates.waitTime <= 0)
        {
            canMove = true;
        }

        if (canMove)
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");

            //Dash
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("hola");
                playerRb2D.AddForce(Vector2.up * dashForce, ForceMode2D.Impulse);
            }
            //Dash

            movement = movementInput.normalized * movementSpeed * Time.deltaTime;

            //Ataque
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Attack());
            }
            //Ataque

            //Para que el jugador mire donde anda
            if (movementInput.x < 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, 0.707106829f, 0.707106829f);
            }
            else if (movementInput.x > 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, -0.707106829f, 0.707106829f);
            }

            if (movementInput.y < 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, -1, 0);
            }
            else if (movementInput.y > 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, 0, 0);
            }

            if (movementInput.y > 0 & movementInput.x < 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, 0.382683426f, 0.923879564f);
            }
            else if (movementInput.y > 0 & movementInput.x > 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, -0.382683426f, 0.923879564f);
            }
            else if (movementInput.y < 0 & movementInput.x < 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, 0.923879564f, 0.382683426f);
            }
            else if (movementInput.y < 0 & movementInput.x > 0)
            {
                transform.GetChild(0).rotation = new Quaternion(0, 0, -0.923879564f, 0.382683426f);
            }
            //Para que el jugador mire donde anda

            //Para que la cabeza mire donde apunte
            Debug.Log(point);

            transform.GetChild(1).LookAt(point);
            //Para que la cabeza mire donde apunte
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

    void OnGUI()
    {
        mousePos.x = Event.current.mousePosition.x;
        mousePos.y = Camera.main.pixelHeight - Event.current.mousePosition.y;
        point = new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane);

    }

    IEnumerator Attack()
    {
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackCollider.SetActive(false);
    }
}
