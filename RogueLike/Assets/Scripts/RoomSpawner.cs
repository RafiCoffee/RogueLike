using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    private int randomRoom;

    public float waitTime = 4f;

    private bool spawned = false;

    private RoomTemplates templates;

    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (spawned == false)
        {
            switch(openingDirection)
            {
                case 1:
                    randomRoom = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 2:
                    randomRoom = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 3:
                    randomRoom = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 4:
                    randomRoom = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 5:
                    randomRoom = Random.Range(0, templates.bottomRooms.Length - 1);
                    Instantiate(templates.bottomRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 6:
                    randomRoom = Random.Range(0, templates.topRooms.Length - 1);
                    Instantiate(templates.topRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 7:
                    randomRoom = Random.Range(0, templates.leftRooms.Length - 1);
                    Instantiate(templates.leftRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;

                case 8:
                    randomRoom = Random.Range(0, templates.rightRooms.Length - 1);
                    Instantiate(templates.rightRooms[randomRoom], transform.position, Quaternion.identity, templates.room.transform);
                    break;
            }
            spawned = true;
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
