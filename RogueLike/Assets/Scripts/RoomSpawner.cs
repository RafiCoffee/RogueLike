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
            if (openingDirection == 1)
            {
                randomRoom = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[randomRoom], transform.position, Quaternion.identity);
            }
            if (openingDirection == 2)
            {
                randomRoom = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[randomRoom], transform.position, Quaternion.identity);
            }
            if (openingDirection == 3)
            {
                randomRoom = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[randomRoom], transform.position, Quaternion.identity);
            }
            if (openingDirection == 4)
            {
                randomRoom = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[randomRoom], transform.position, Quaternion.identity);
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
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
