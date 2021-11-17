using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Dungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
