using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject menuCanvas;
    public static GameManager instance = null;

    // Game status bools
    public bool inGame, paused, gameOver, restart;

    public int startLives;

    // Cross Game GUI Options
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
    public GUIText livesText;
    private string gameOverMessage = "Game Over!";

    // Player attributes
    private int lives;
    private int score;

    // Making public to test spawn time;
    public float waitOnRespawn;

    // Ship selection logic
    // TODO: In future ship default should be saved across sessions.4
    public GameObject[] ships;
    public GameObject defaultShip;
    private GameObject activeShip;

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

    public void Start()
    {
        // TODO: Confirm need for instantiating int value (does it not default to 0?)
        SetDefaults();
    }

    public void SetDefaults()
    {
        score = 0;
        lives = startLives;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        livesText.text = "";
        scoreText.text = "";
        activeShip = defaultShip;
    }

    // Spawns the active (selected) ship - call this from level controllers
    public void SpawnActiveShip()
    {
        activeShip.SetActive(true);
    }

    public void DisableActiveShip()
    {
        activeShip.SetActive(false);
    }

    public void ChangeShip(GameObject selectedShip)
    {
        if (activeShip == selectedShip)
        {
            // Do nothing if already actvie (don't need active context flickers)
            return;
        }
        if (inGame)
        {
            // If in game a ship is already active so disable it and enabled the new selection.
            // Also need to update the newly activated ship transform position to put it where the
            // previously active ship was.
            selectedShip.transform.position = activeShip.transform.position;
            selectedShip.transform.rotation = activeShip.transform.rotation;
            activeShip.SetActive(false);
            selectedShip.SetActive(true);
        }
        activeShip = selectedShip;
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

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives()
    {
        livesText.text = "Lives: " + lives;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void PauseGame()
    {
        Debug.Log(string.Format("Number of child objects: {0}", menuCanvas.transform.childCount));
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        paused = true;
        if (gameOver)
        {
            // Little bit of logic to clean up UI when
            // open menu and "Game Over!" message is on screen
            gameOverText.text = "";
        }
        //Disable scripts that still work while timescale is set to 0
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        paused = false;
        if (gameOver)
        {
            // If game is over, redisplay the game over text when closing
            // menu.
            gameOverText.text = gameOverMessage;
        }
        //enable the scripts again
    }

    public void BackToStart()
    {
        inGame = false;
        DisableActiveShip();
        Time.timeScale = 1;
        paused = false;
        // Going back to start screen so don't need GUI text / player attributes etc.
        SetDefaults();
        //enable the scripts again
    }

    public void PlayerKilled()
    {
        // Pretend ship destroyed
        activeShip.SetActive(false);
        // Put ship back at origin for new life/level
        activeShip.transform.position = new Vector3(0, 0, 0);
        // Check players amount of lives to decided on ending game or restarting level.
        if (lives == 0)
        {
            GameOver();
        }
        else
        {
            lives -= 1;
            UpdateLives();
            // TODO: This should be smart enough to restart the current loaded scene
            StartCoroutine(RestartLevel());
        }
    }

    public void AllowRestart()
    {
        restartText.text = "Press 'R' to restart";
        restart = true;
    }

    public IEnumerator RestartLevel()
    {
        // When restarting from gameOver context - need to reset attributes/gui text etc.
        if (gameOver)
        {
            SetDefaults();
            UpdateLives();
            UpdateScore();
        }
        else
        {
            // If restarting on Death (automatic) wait a few seconds
            yield return new WaitForSeconds(waitOnRespawn);
        }
        // Only specifying the sceneName or sceneBuildIndex will load the scene with the Single mode
        SceneManager.LoadScene("SpaceShooter");
    }

    public void GameOver()
    {
        // Function to handle logic on game end
        // Rest player attributes for new Games
        // TODO: Track score into leaderboard (get player names needs to be done)
        gameOverText.text = gameOverMessage;
        gameOver = true;
    }
}
