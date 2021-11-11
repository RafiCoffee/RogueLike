using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EntryRoom") == false && collision.CompareTag("Player") == false && collision.CompareTag("DungeonLimit") == false && collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
