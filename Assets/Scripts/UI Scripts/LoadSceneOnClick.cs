using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    // Standard Load scene by index passed from unity (this is for loading into game levels)
    // Use `LoadByIndexPaused` if returning to start screen scene.
    public void LoadByIndex(int sceneIndex)
    {
        GameManager.instance.UpdateLives();
        GameManager.instance.UpdateScore();
        SceneManager.LoadScene(sceneIndex);
        GameManager.instance.SpawnActiveShip();
    }
    
    // Loads scene by index passed from Unity but handles logic specific to being in
    // a paused game context (turning off menu etc.)
    public void LoadByIndexPaused(int sceneIndex)
    {
        // Load scene can be returning from game to main menu in which case the game is in a paused state
        // i.e. Time.timeScale = 0; so need to undo this
        // TODO: Unity can't handle updating params for default load scene function - research this
        GameManager.instance.BackToStart();
        SceneManager.LoadScene(sceneIndex);
    }
}
