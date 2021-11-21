using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public bool isMapOpen = false;

    public GameObject pauseMenu;
    public GameObject MiniMap;
    public GameObject HUD;

    public Texture2D normalCursor;
    public Texture2D shotCursor;

    private PlayerController2DVC playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Jugador2DVC").GetComponent<PlayerController2DVC>();

        pauseMenu.SetActive(false);
        MiniMap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused == false)
            {
                Time.timeScale = 0;
                playerScript.canMove = false;
                pauseMenu.SetActive(true);
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
                isPaused = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.M) && playerScript.haveMap)
        {
            if (isMapOpen == false)
            {
                playerScript.canMove = false;
                MiniMap.SetActive(true);
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
                HUD.SetActive(false);
                isMapOpen = true;
            }
            else
            {
                playerScript.canMove = true;
                MiniMap.SetActive(false);
                Cursor.SetCursor(shotCursor, Vector2.zero, CursorMode.Auto);
                HUD.SetActive(true);
                isMapOpen = false;
            }
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        playerScript.canMove = true;
        pauseMenu.SetActive(false);
        Cursor.SetCursor(shotCursor, Vector2.zero, CursorMode.Auto);
        isPaused = false;
    }
}
