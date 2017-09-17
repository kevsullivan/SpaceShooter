using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnpauseGame : MonoBehaviour
{

    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Should only be attached as component when game is paused (Time.timeScale == 0)
    public void Unpause()
    {
        Time.timeScale = 1;
        gameController.ContinueGame();
    }
}
