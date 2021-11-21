using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetect : MonoBehaviour
{
    private RoomTemplates templates;

    private bool spawned = false;

    public int direccion = 5;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    void Update()
    {
        if (templates.waitTime <= 0 && spawned == false)
        {
            switch(gameObject.tag)
            {
                case "WallDetectT":
                    Instantiate(templates.closedWall[0], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;

                case "WallDetectB":
                    Instantiate(templates.closedWall[1], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;

                case "WallDetectR":
                    Instantiate(templates.closedWall[2], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;

                case "WallDetectL":
                    Instantiate(templates.closedWall[3], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallDetectT")|| collision.CompareTag("WallDetectB")|| collision.CompareTag("WallDetectR")|| collision.CompareTag("WallDetectL"))
        {
            switch (collision.gameObject.tag)
            {
                case "WallDetectT":
                    spawned = true;
                    direccion = 0;
                    break;

                case "WallDetectB":
                    spawned = true;
                    direccion = 3;
                    break;

                case "WallDetectR":
                    spawned = true;
                    direccion = 1;
                    break;

                case "WallDetectL":
                    spawned = true;
                    direccion = 2;
                    break;
            }
        }
        /*else if (collision.gameObject.layer == 6)
        {
            switch (gameObject.tag)
            {
                case "WallDetectT":
                    Instantiate(templates.closedWall[0], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;

                case "WallDetectB":
                    Instantiate(templates.closedWall[1], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;

                case "WallDetectR":
                    Instantiate(templates.closedWall[2], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;

                case "WallDetectL":
                    Instantiate(templates.closedWall[3], transform.position, Quaternion.identity, templates.room.transform);
                    Destroy(gameObject);
                    break;
            }
            spawned = true;
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (templates.waitTime < 1 && collision.CompareTag("DungeonLimit") && collision.gameObject.layer != 9 && collision.gameObject.layer != 6)
        {
            Destroy(gameObject);
        }
    }
}
