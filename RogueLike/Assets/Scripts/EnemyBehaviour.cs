using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private int rutina;
    public int speed;
    private int direction;
    private int hola;

    private float cronometro;
    public float visionRadius;

    public bool isMoving = false;
    public bool isAtacking;

    private GameObject target;
    private GameObject rango;

    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        target = GameObject.Find("Jugador2DVC");
    }

    // Update is called once per frame
    void Update()
    {
        Comportamientos();

        /*if (isWatchingPlayer == false)
        {
            isWatchingPlayer = true;
        }
        else
        {
            isWatchingPlayer = false;
        }*/
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    public void Comportamientos()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > visionRadius || Mathf.Abs(transform.position.y - target.transform.position.y) > visionRadius)
        {
            cronometro += Time.deltaTime;
            if (cronometro >= 3)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    isMoving = false;
                    break;

                case 1:
                    direction = Random.Range(0, 4);
                    rutina++;
                    break;

                case 2:

                    switch (direction)
                    {
                        case 0:
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            isMoving = true;
                            break;

                        case 1:
                            transform.rotation = Quaternion.Euler(0, 0, 90);
                            isMoving = true;
                            break;

                        case 2:
                            transform.rotation = Quaternion.Euler(0, 0, 180);
                            isMoving = true;
                            break;

                        case 3:
                            transform.rotation = Quaternion.Euler(0, 0, 270);
                            isMoving = true;
                            break;
                    }

                    break;
            }
        }
        else
        {
            if (transform.position.x < target.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            if (transform.position.x == target.transform.position.x || transform.position.y < target.transform.position.y)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.position.x == target.transform.position.x || transform.position.y > target.transform.position.y)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 6 || collision.collider.gameObject.layer == 9 || collision.collider.gameObject.layer == 10)
        {
            rutina = 1;
        }
    }
}
