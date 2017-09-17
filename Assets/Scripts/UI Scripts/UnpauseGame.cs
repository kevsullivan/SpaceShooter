using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnpauseGame : MonoBehaviour
{
    // Used for unpausing from menu resume game button click
    public void Unpause()
    {
        GameManager.instance.ContinueGame();
    }
}
