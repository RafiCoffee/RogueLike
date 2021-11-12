using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetect : MonoBehaviour
{
    private RoomTemplates templates;

    private bool spawned = false;

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
                    break;

                case "WallDetectB":
                    Instantiate(templates.closedWall[1], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case "WallDetectR":
                    Instantiate(templates.closedWall[2], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case "WallDetectL":
                    Instantiate(templates.closedWall[3], transform.position, Quaternion.identity, templates.room.transform);
                    break;
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallDetectT") || collision.CompareTag("WallDetectB") || collision.CompareTag("WallDetectR") || collision.CompareTag("WallDetectL"))
        {
            Destroy(gameObject);
        }
    }
}
