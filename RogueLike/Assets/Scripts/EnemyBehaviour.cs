using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField, Range(0, 500)]
    public int vida = 50;
    [SerializeField, Range(0, 50)]
    public int meleeDamage = 20;

    public int enemyRoom;
    public int dashForce;

    public bool isMoving = false;
    public bool isAtacking;
    public bool isBoss = false;
    public bool invencible = false;

    private GameObject target;
    public GameObject visual;

    private Vector2 initialPosition;
    public Vector2 followPlayer;
    private Vector2 colision;

    private Slider healthSlider;

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
        if (isBoss)
        {
            healthSlider = GameObject.Find("BossSlider").GetComponent<Slider>();

            healthSlider.maxValue = vida;
            healthSlider.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer = target.transform.position - transform.position;

        transform.up = followPlayer;

        if (enemyRoom == playerScript.room && isBoss)
        {
            healthSlider.gameObject.SetActive(true);

            healthSlider.value = vida;
        }

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (enemyRoom == playerScript.room)
        {
            StartCoroutine(Wait());
            if (isMoving)
            {
                enemyRb.MovePosition((Vector2)transform.position + (followPlayer / 1.5f * Time.deltaTime));
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
        if (collision.collider.CompareTag("Attack") && invencible == false)
        {
            vida = vida - playerScript.meleeDamage;

            colision = collision.GetContact(0).normal;
            invencible = true;
            StartCoroutine(Retroceso());
        }

        if (collision.collider.CompareTag("Bullet") && invencible == false)
        {
            vida = vida - playerScript.bulletDamage;

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
