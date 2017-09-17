using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    private GameController gameController;

    public void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else
        {
            // Can occur when switching from In Game to Main Menu (TODO: maybe address this)
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadByIndexPaused(int sceneIndex)
    {
        // Load scene can be returning from game to main menu in which case the game is in a paused state
        // i.e. Time.timeScale = 0; so need to undo this
        // TODO: Unity can't handle updating params for default load scene function - research this
        gameController.ContinueGame();
        SceneManager.LoadScene(sceneIndex);
    }
}
