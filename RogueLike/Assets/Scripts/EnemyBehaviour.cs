using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    public int vida = 10;

    public int enemyRoom;
    public int dashForce;

    public bool isMoving = false;
    public bool isAtacking;
    private bool invencible = false;

    private GameObject target;
    public GameObject visual;

    private Vector2 initialPosition;
    private Vector2 followPlayer;
    private Vector2 colision;

    private Rigidbody2D enemyRb;

    private AddRooms addRoomsScript;
    private PlayerController2DVC playerScript;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        enemyRb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Jugador2DVC");

        addRoomsScript = GameObject.Find("Entry Room").GetComponent<AddRooms>();
        playerScript = GameObject.Find("Jugador2DVC").GetComponent<PlayerController2DVC>();
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer = target.transform.position - transform.position;

        if (followPlayer.x < 0 && followPlayer.x < followPlayer.y)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (followPlayer.x > 0 && followPlayer.x > followPlayer.y)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (followPlayer.y < 0 && followPlayer.y < followPlayer.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (followPlayer.y > 0 && followPlayer.y > followPlayer.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void FixedUpdate()
    {
        if (enemyRoom == playerScript.room)
        {
            StartCoroutine(Wait());
            if (isMoving)
            {
                enemyRb.MovePosition((Vector2)transform.position + (followPlayer / 2 * Time.deltaTime));
            }
        }
        else
        {
            isMoving = false;
            enemyRb.MovePosition(initialPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Attack") && invencible == false || collision.collider.CompareTag("Bullet") && invencible == false)
        {
            colision = collision.GetContact(0).normal;
            invencible = true;
            StartCoroutine(Retroceso());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            addRoomsScript = collision.gameObject.GetComponent<AddRooms>();

            enemyRoom = addRoomsScript.room;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.8f);
        isMoving = true;
    }

    IEnumerator Retroceso()
    {
        isMoving = false;
        enemyRb.velocity = colision * dashForce;
        yield return new WaitForSeconds(0.15f);
        enemyRb.velocity = Vector2.zero;
        isMoving = true;
        StartCoroutine(Invencible());
    }

    IEnumerator Invencible()
    {
        visual.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        visual.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        visual.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        visual.SetActive(true);
        invencible = false;
    }
}
