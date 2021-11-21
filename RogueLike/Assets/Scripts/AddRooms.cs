using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    public int room;
    public int direccion;
    public int entities = 0;

    private bool direccionSet = false;
    private bool isPlayer = false;
    public bool ocupado = false;

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

        if (templates.waitTime <= 0 && entities > 1 && isPlayer && gameObject.tag != "EntryRoom")
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        if (templates.waitTime <= 0 && entities == 1 && isPlayer && gameObject.tag != "EntryRoom")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag != "BossRoom")
        {
            if (collision.gameObject.layer == 10)
            {
                entities++;
            }
            
            if (collision.gameObject.layer == 7)
            {
                entities++;
                isPlayer = true;
            }
        }

        if (collision.gameObject.layer == 20 && collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            ocupado = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag != "BossRoom")
        {
            if (collision.gameObject.layer == 7 || collision.gameObject.layer == 10)
            {
                entities--;
            }
        }
    }
}
