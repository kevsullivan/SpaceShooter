using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadByIndexPaused(int sceneIndex)
    {
        // Load scene can be returning from game to main menu in which case the game is in a paused state
        // i.e. Time.timeScale = 0; so need to undo this
        // TODO: Unity can't handle updating params for default load scene function - research this
        GameManager.instance.BackToStart();
        SceneManager.LoadScene(sceneIndex);
    }
}
