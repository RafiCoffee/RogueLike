using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    private int randomRoom;
    private int closedRoom = 1;

    public float waitTime = 2f;

    private bool spawned = false;

    private RoomTemplates templates;

    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        if (templates.rooms.Count == templates.roomMin)
        {
            closedRoom = 0;
        }


        if (templates.rooms.Count >= templates.roomMax)
        {
            spawned = true;
        }
        else
        {
            Invoke("Spawn", 0.1f);
        }
    }

    void Spawn()
    {
        if (spawned == false)
        {
            switch(openingDirection)
            {
                case 1:
                    randomRoom = Random.Range(0, templates.bottomRooms.Length - closedRoom);
                    Instantiate(templates.bottomRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 2:
                    randomRoom = Random.Range(0, templates.topRooms.Length - closedRoom);
                    Instantiate(templates.topRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 3:
                    randomRoom = Random.Range(0, templates.leftRooms.Length - closedRoom);
                    Instantiate(templates.leftRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 4:
                    randomRoom = Random.Range(0, templates.rightRooms.Length - closedRoom);
                    Instantiate(templates.rightRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;
            }
            spawned = true;
        }
    }

    void Update()
    {
        if (templates.rooms.Count == templates.roomMax)
        {
            spawned = true;
        }

        if (templates.rooms.Count >= templates.roomMin)
        {
            closedRoom = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spawn Point"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, templates.room.transform);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
