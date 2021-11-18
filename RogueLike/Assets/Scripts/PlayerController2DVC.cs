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
    [SerializeField, Range(0, 10)] 
    public int vida = 10;
    [SerializeField, Range(0, 100)]
    public int dashForce = 30;
    [SerializeField, Range(0, 10)]
    public int meleeDamage = 2;
    [SerializeField, Range(0, 10)]
    public int bulletDamage = 3;

    Vector2 movement;
    Vector2 movementInput;
    Vector2 colision;

    public int room;
    private int direction = 1;
    public int ammo = 10;

    public float movementSpeed = 30f;
    private float dashTime;
    public float StartDashTime;
    public float dashCoolDown = 1.5f;
    private float dashTimer = 0;
    public float bulletCooldown = 0.5f;

    private bool oneTime = true;
    public bool canMove = false;
    public bool canDash = false;
    private bool isDashing = false;
    private bool invencible = false;

    private Rigidbody2D playerRb2D;
    private Slider dashSlider;

    private GameObject attackCollider;
    private GameObject bullet;
    public GameObject visual;

    private RoomTemplates templates;
    private BulletPool bulletPool;
    private AddRooms addRoomScript;

    public TextMeshProUGUI ammoText;

    private Stopwatch timerDash = new Stopwatch();
    private Stopwatch timerBullet = new Stopwatch();
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        attackCollider = GameObject.Find("AttackCollider");
        bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>();
        dashSlider = GameObject.Find("Dash").GetComponent<Slider>();

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            addRoomScript = GameObject.Find("Entry Room").GetComponent<AddRooms>();
        }

        attackCollider.SetActive(false);

        dashTime = StartDashTime;

        timerDash.Start();
        timerBullet.Start();
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Attack());
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
        if (collision.collider.CompareTag("Enemy") && invencible == false)
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
            addRoomScript = collision.gameObject.GetComponent<AddRooms>();

            room = addRoomScript.room;
        }
    }

    public void Shoot()
    {
        bullet = bulletPool.GetPooledObject();
        bullet.SetActive(true);
        bullet.transform.position = transform.GetChild(1).GetChild(1).GetChild(0).position;
        bullet.transform.rotation = transform.GetChild(1).GetChild(1).GetChild(0).rotation;

        ammo--;
        timerBullet.Restart();
    }

    IEnumerator Attack()
    {
        invencible = true;
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        attackCollider.SetActive(false);
        invencible = false;
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
