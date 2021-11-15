using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    public int room;

    private RoomTemplates templates;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        templates.rooms.Add(this.gameObject);

        room = templates.rooms.Count;
    }
}
