using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] bossRooms;

    public GameObject closedRoom;
    public GameObject[] closedWall;
    public GameObject room;

    public List<GameObject> rooms;

    public int roomMax;
    public int roomMin;
    public float waitTime;
    private bool spawnedEnemies = false;
    public GameObject boss;
    public GameObject spawnEnemies;

    public int enemyRooms;
    private bool enemyRoomsSet = false;
    private int spawnRoom;
    private bool spawnedBossRoom = false;

    private Vector2 spawnBossRoom;

    private AddRooms addRoomsScript;
    void Update()
    {
        if (waitTime <= 0 && spawnedEnemies == false)
        {
            if (spawnedBossRoom == false)
            {
                addRoomsScript = rooms[rooms.Count - 1].GetComponent<AddRooms>();
                spawnBossRoom = rooms[rooms.Count - 1].transform.position;
                Destroy(rooms[rooms.Count - 1]);
                rooms.Remove(rooms[rooms.Count - 1]);

                Instantiate(bossRooms[addRoomsScript.direccion], spawnBossRoom, Quaternion.identity);
                spawnedBossRoom = true;
            }

            if (enemyRoomsSet == false)
            {
                enemyRooms = Random.Range(2, rooms.Count - 4);
                enemyRoomsSet = true;
            }

            if (enemyRoomsSet)
            {
                for (int i = 0; i < enemyRooms + 1; i++)
                {
                    spawnRoom = Random.Range(0, 2);
                    if (spawnRoom == 0)
                    {
                        if (i == 0)
                        {
                            i = 1;
                        }
                        else
                        {
                            Instantiate(spawnEnemies, rooms[i].transform.position, Quaternion.identity);
                        }
                    }
                    else if (spawnRoom == 1)
                    {
                        i--;
                    }
                }
            }

            if (spawnedBossRoom)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (i == rooms.Count - 1)
                    {
                        Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                        spawnedEnemies = true;
                    }
                }
            }

        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
