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
    public GameObject spawners;
    public GameObject objects;

    public List<GameObject> rooms;

    public int roomMax;
    public int roomMin;
    private int buff = 0;
    public float waitTime;
    private bool spawnedEnemies = false;
    public GameObject boss;
    public GameObject spawnEnemies;
    public GameObject map;
    public GameObject buffos;

    public int enemyRooms;
    private int enemiesSpawned = 0;
    private bool enemyRoomsSet = false;
    private int spawnRoom;
    private bool spawnedBossRoom = false;
    private bool buffosSet = false;
    private bool mapSet = false;

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

                Instantiate(bossRooms[addRoomsScript.direccion], spawnBossRoom, Quaternion.identity, room.transform);
                spawnedBossRoom = true;
            }

            if (enemyRoomsSet == false)
            {
                enemyRooms = Random.Range(3, rooms.Count - 5);
                enemyRoomsSet = true;
            }

            if (buffosSet == false)
            {
                for (int i = 0; i < 100; i++)
                {
                    addRoomsScript = rooms[i].GetComponent<AddRooms>();
                    if (buff == 3)
                    {
                        buffosSet = true;
                        i = 101;
                    }
                    else
                    {
                        spawnRoom = Random.Range(0, 2);
                        if (spawnRoom == 0)
                        {
                            if (i == 0 || i == rooms.Count - 1)
                            {
                                i = 1;
                            }
                            else
                            {
                                Instantiate(buffos, rooms[i].transform.position + new Vector3(0, 0, -1), Quaternion.identity, objects.transform);
                                addRoomsScript.ocupado = true;
                                buff++;
                            }
                        }
                        else if (spawnRoom == 1)
                        {
                            i++;
                        }
                    }
                }
            }

            if (mapSet == false && buffosSet)
            {
                for (int i = 0; i < 100; i++)
                {
                    addRoomsScript = rooms[i].GetComponent<AddRooms>();
                    if (buff == 0)
                    {
                        mapSet = true;
                        i = 101;
                    }
                    else
                    {
                        spawnRoom = Random.Range(0, 2);
                        if (spawnRoom == 0)
                        {
                            if (i == 0 || i == rooms.Count - 1)
                            {
                                i = 1;
                            }
                            else
                            {
                                if (addRoomsScript.ocupado)
                                {
                                    i++;
                                }
                                else
                                {
                                    Instantiate(map, rooms[i].transform.position + new Vector3(0, 0, -1), Quaternion.identity, objects.transform);
                                    addRoomsScript.ocupado = true;
                                    buff = 0;
                                }
                            }
                        }
                        else if (spawnRoom == 1)
                        {
                            i++;
                        }
                    }
                }
            }

            if (enemyRoomsSet && buffosSet && mapSet)
            {
                for (int i = 0; enemiesSpawned < enemyRooms; i++)
                {
                    addRoomsScript = rooms[i].GetComponent<AddRooms>();
                    if (i >= rooms.Count - 1)
                    {
                        i = 0;
                    }
                    else
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
                                if (addRoomsScript.ocupado)
                                {
                                    i++;
                                }
                                else
                                {
                                    Instantiate(spawnEnemies, rooms[i].transform.position, Quaternion.identity, spawners.transform);
                                    enemiesSpawned++;
                                }
                            }
                        }
                        else if (spawnRoom == 1)
                        {
                            i++;
                        }
                    }
                }
            }

            if (spawnedBossRoom)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (i == rooms.Count - 1)
                    {
                        Instantiate(boss, rooms[i].transform.position, Quaternion.identity, spawners.transform);
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
