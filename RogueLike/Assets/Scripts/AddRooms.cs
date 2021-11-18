using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    public int room;
    public int direccion;

    private bool direccionSet = false;

    private RoomTemplates templates;
    private WallDetect wallDetectScript;

    void Awake()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        templates.rooms.Add(this.gameObject);

        room = templates.rooms.Count;

        if (gameObject.tag == "BossRoom")
        {
            direccionSet = true;
        }
    }

    void Update()
    {
        if (templates.waitTime <= 0 && room == templates.rooms.Count && direccionSet == false)
        {
            wallDetectScript = GetComponentInChildren<WallDetect>();
            direccion = wallDetectScript.direccion;
            direccionSet = true;
        }
    }
}
