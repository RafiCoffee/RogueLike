using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController2DVC : MonoBehaviour
{
    [SerializeField, Range(0, 100)] 
    public int vida = 50;
    [SerializeField, Range(0, 100)]
    public int dashForce = 30;
    [SerializeField, Range(0, 50)]
    public int meleeDamage = 5;
    [SerializeField, Range(0, 50)]
    public int bulletDamage = 10;

    Vector2 movement;
    Vector2 movementInput;
    Vector2 colision;

    public int room;
    private int direction = 1;
    public int ammo = 10;
    public int buffo = 0; 

    public float movementSpeed = 30f;
    private float dashTime;
    public float StartDashTime;
    public float dashCoolDown = 1.5f;
    private float dashTimer = 0;
    public float bulletCooldown = 0.5f;
    public float meleeCooldown = 0.8f;

    private bool oneTime = true;
    public bool canMove = false;
    public bool canDash = false;
    private bool isDashing = false;
    private bool invencible = false;
    public bool isAttacking = false;
    public bool haveMap = false;
    private bool buffoSet = false;
    public bool isDead = false;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D playerBc2D;
    private Slider dashSlider;

    private GameObject bullet;
    public GameObject visual;
    public GameObject upgrade1;
    public GameObject upgrade2;
    public GameObject upgrade3;

    private RoomTemplates templates;
    private BulletPool bulletPool;
    private AddRooms addRoomScript;

    public TextMeshProUGUI ammoText;
    private Animator playerAnim;

    private Stopwatch timerDash = new Stopwatch();
    private Stopwatch timerBullet = new Stopwatch();
    private Stopwatch timerMelee = new Stopwatch();
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerBc2D = GetComponent<BoxCollider2D>();
        bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        dashSlider = GameObject.Find("Dash").GetComponent<Slider>();
        playerAnim = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            addRoomScript = GameObject.Find("Entry Room").GetComponent<AddRooms>();
        }

        dashTime = StartDashTime;
        buffo = 0;

        timerDash.Start();
        timerBullet.Start();
        timerMelee.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (templates.waitTime <= 0 && oneTime)
            {
                canMove = true;
                canDash = true;
                oneTime = false;
            }
        }
        else
        {
            canMove = true;
            canDash = true;
            oneTime = false;
        }

        dashTimer += Time.deltaTime;

        dashSlider.value = dashTimer;

        if (canDash)
        {
            //Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) & timerDash.ElapsedMilliseconds / 1000 > dashCoolDown)
            {
                gameObject.layer = 16;
                canMove = false;
                if (direction != 0)
                {
                    if (dashTime > 0)
                    {
                        isDashing = true;
                        if (direction == 1)
                        {
                            playerRb2D.velocity = Vector2.up.normalized * dashForce;
                        }
                        else if (direction == 2)
                        {
                            playerRb2D.velocity = Vector2.down.normalized * dashForce;
                        }
                        else if (direction == 3)
                        {
                            playerRb2D.velocity = Vector2.right.normalized * dashForce;
                        }
                        else if (direction == 4)
                        {
                            playerRb2D.velocity = Vector2.left.normalized * dashForce;
                        }
                        else if (direction == 5)
                        {
                            playerRb2D.velocity = new Vector2(-1, 1).normalized * dashForce;
                        }
                        else if (direction == 6)
                        {
                            playerRb2D.velocity = Vector2.one.normalized * dashForce;
                        }
                        else if (direction == 7)
                        {
                            playerRb2D.velocity = -Vector2.one.normalized * dashForce;
                        }
                        else if (direction == 8)
                        {
                            playerRb2D.velocity = new Vector2(1, -1).normalized * dashForce;
                        }
                    }
                }
            }

            if (isDashing)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = StartDashTime;
                    playerRb2D.velocity = Vector2.zero;
                    canMove = true;
                    isDashing = false;
                    dashTimer = 0;
                    gameObject.layer = 7;
                    timerDash.Restart();
                }
            }
            //Dash
        }

        if (canMove)
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");

            movement = movementInput.normalized * movementSpeed * Time.deltaTime;

            //Ataque
            if (Input.GetKeyDown(KeyCode.Space) && timerMelee.ElapsedMilliseconds / 1000 > meleeCooldown)
            {
                playerAnim.SetTrigger("Attack");
                timerMelee.Restart();
            }
            //Ataque

            //Disparar
            if (Input.GetMouseButtonDown(0) && timerBullet.ElapsedMilliseconds / 1000 > bulletCooldown && ammo != 0)
            {
                Shoot();
            }
            //Disparar

            //Para que el jugador mire donde anda
            if (movementInput.x < 0)
            {
                direction = 4;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (movementInput.x > 0)
            {
                direction = 3;
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }

            if (movementInput.y < 0)
            {
                direction = 2;
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (movementInput.y > 0)
            {
                direction = 1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (movementInput.y > 0 & movementInput.x < 0)
            {
                direction = 5;
                transform.rotation = Quaternion.Euler(0, 0, 45);
            }
            else if (movementInput.y > 0 & movementInput.x > 0)
            {
                direction = 6;
                transform.rotation = Quaternion.Euler(0, 0, 315);
            }
            else if (movementInput.y < 0 & movementInput.x < 0)
            {
                direction = 7;
                transform.rotation = Quaternion.Euler(0, 0, 135);
            }
            else if (movementInput.y < 0 & movementInput.x > 0)
            {
                direction = 8;
                transform.rotation = Quaternion.Euler(0, 0, 225);
            }
            //Para que el jugador mire donde anda
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ammoText.text = ammo.ToString();
        }

        if (vida <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            gameObject.SetActive(false);
        }

        switch(buffo)
        {
            case 1:
                if (buffoSet == false)
                {
                    upgrade1.SetActive(true);
                    meleeDamage = 10;
                    bulletDamage = 15;
                    buffoSet = true;
                }
                break;

            case 2:
                if (buffoSet)
                {
                    upgrade1.SetActive(false);
                    upgrade2.SetActive(true);
                    meleeDamage = 15;
                    bulletDamage = 20;
                    buffoSet = false;
                }
                break;

            case 3:
                if(buffoSet == false)
                {
                    upgrade2.SetActive(false);
                    upgrade3.SetActive(true);
                    meleeDamage = 20;
                    bulletDamage = 25;
                    buffoSet = true;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            playerRb2D.MovePosition(playerRb2D.position + movement);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && invencible == false && isAttacking == false)
        {
            colision = collision.GetContact(0).normal;
            vida = vida - 5;
            invencible = true;
            StartCoroutine(Retroceso());
        }

        if (collision.collider.CompareTag("Fire") && invencible == false && isAttacking == false)
        {
            colision = collision.GetContact(0).normal;
            vida = vida - 10;
            invencible = true;
            StartCoroutine(Retroceso());
        }

        if (collision.collider.CompareTag("Boss") && invencible == false && isAttacking == false)
        {
            colision = collision.GetContact(0).normal;
            vida = vida - 10;
            invencible = true;
            StartCoroutine(Retroceso());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            addRoomScript = collision.gameObject.GetComponent<AddRooms>();

            room = addRoomScript.room;
        }


        if (collision.CompareTag("Map"))
        {
            haveMap = true;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Buffo"))
        {
            buffo++;
            Destroy(collision.gameObject);
        }
    }

    public void Shoot()
    {
        bullet = bulletPool.GetPooledObject();
        bullet.SetActive(true);
        bullet.transform.position = transform.GetChild(1).GetChild(4).GetChild(0).position;
        bullet.transform.rotation = transform.GetChild(1).GetChild(4).GetChild(0).rotation;

        ammo--;
        timerBullet.Restart();
    }

    IEnumerator Retroceso()
    {
        canMove = false;
        playerRb2D.velocity = colision * dashForce;
        yield return new WaitForSeconds(0.15f);
        playerRb2D.velocity = Vector2.zero;
        canMove = true;
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
