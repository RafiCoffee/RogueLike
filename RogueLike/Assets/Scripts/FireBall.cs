using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float height = 3;
    public float speed = 5;

    private bool isOnTop = false;

    private CircleCollider2D fireBallCc;

    private GameObject player;

    public Vector3 target;
    public Vector2 middle;

    void Awake()
    {
        player = GameObject.Find("Jugador2DVC");
        fireBallCc = GetComponent<CircleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
        fireBallCc.enabled = false;
    }

    void OnEnable()
    {
        target = player.transform.position;
        fireBallCc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnTop == false)
        {
            fireBallCc.enabled = false;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3 (transform.position.x, transform.position.y, -height), speed * Time.deltaTime);
            speed -= 6 * Time.deltaTime;
            if (speed <= 5)
            {
                speed = 5;
            }
            if (transform.position == new Vector3(transform.position.x, transform.position.y, -height))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y , -height);
                isOnTop = true;
            }
        }
        else
        {
            speed += 8 * Time.deltaTime;
            if (speed >= 10)
            {
                speed = 10;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position == target)
            {
                fireBallCc.enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isOnTop = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NormalRoom"))
        {
            isOnTop = false;
            gameObject.SetActive(false);
        }
    }
}
