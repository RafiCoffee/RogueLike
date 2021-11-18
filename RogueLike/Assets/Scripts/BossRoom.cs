using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        door = transform.GetChild(transform.childCount - 2).gameObject;

        door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            door.SetActive(true);
        }
    }
}
