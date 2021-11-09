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
            Instantiate(templates.closedWall, transform.position, Quaternion.identity, templates.room.transform);
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallDetect"))
        {
            Destroy(gameObject);
        }
    }
}
