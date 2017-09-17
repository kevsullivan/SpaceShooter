using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject mainMenu;
    public static GameManager instance = null;

    private bool paused;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // When loading a new level all objects in the scene are destroyed,
        // then the objects in the new level are loaded. In order to preserve an
        // object during level loading call DontDestroyOnLoad on it. If the object
        // is a component or game object then its entire transform hierarchy will not be destroyed either.
        DontDestroyOnLoad(gameObject);
    }

    public void HandleMenu()
    {
        if (paused)
        {
            ContinueGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        paused = true;
        //Disable scripts that still work while timescale is set to 0
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        paused = false;
        //enable the scripts again
    }

    public void BackToStart()
    {
        Time.timeScale = 1;
        paused = false;
        //enable the scripts again
    }
}
