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

    private float meleeCooldown;

    public bool isMoving = false;
    public bool isAttack = false;
    public bool isBoss = false;
    public bool isFurnace = false;
    public bool invencible = false;
    private bool shooted = false;

    private GameObject target;
    public GameObject visual;
    private GameObject fireBall;

    private Vector2 initialPosition;
    public Vector2 followPlayer;
    private Vector2 colision;

    private Slider healthSlider;

    private Rigidbody2D enemyRb;
    private Animator enemyAnim;
    public Animation bossAnim;

    private AddRooms addRoomsScript;
    private PlayerController2DVC playerScript;
    private BulletPool fireBallPool;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        target = GameObject.Find("Jugador2DVC");

        addRoomsScript = GameObject.Find("Entry Room").GetComponent<AddRooms>();
        playerScript = GameObject.Find("Jugador2DVC").GetComponent<PlayerController2DVC>();

        if (isBoss)
        {
            healthSlider = GameObject.Find("BossSlider").GetComponent<Slider>();
            bossAnim = transform.GetChild(1).GetComponent<Animation>();

            healthSlider.maxValue = vida;
            healthSlider.gameObject.SetActive(false);
        }

        if (isFurnace)
        {
            fireBallPool = GameObject.Find("FireBallPool").GetComponent<BulletPool>();
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
            playerScript.ammo = playerScript.ammo + 3;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isFurnace == false)
        {
            if (enemyRoom == playerScript.room)
            {
                StartCoroutine(Wait());
                if (isMoving)
                {
                    if (isBoss)
                    {
                        bossAnim.Play("Walk");
                    }
                    enemyRb.MovePosition((Vector2)transform.position + (followPlayer / 1.5f * Time.deltaTime));
                }
            }
            else
            {
                isMoving = false;
                enemyRb.MovePosition(initialPosition);
            }
        }
        else
        {
            if(enemyRoom == playerScript.room)
            {
                StartCoroutine(WaitFurnace());
                if(isAttack == false)
                {
                    isAttack = true;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 15 && invencible == false)
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

    public void Shoot()
    {
        fireBall = fireBallPool.GetPooledObject();
        fireBall.SetActive(true);
        fireBall.transform.position = transform.GetChild(1).position;
        fireBall.transform.rotation = Quaternion.identity;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        isMoving = true;
    }

    IEnumerator WaitFurnace()
    {
        yield return new WaitForSeconds(1f);
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

    IEnumerator Attack()
    {
        meleeCooldown = Random.Range(1f, 4f);
        if (shooted == false)
        {
            enemyAnim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.35f);
            Shoot();
            shooted = true;
        }
        yield return new WaitForSeconds(meleeCooldown);
        isAttack = false;
        shooted = false;
    }
}
