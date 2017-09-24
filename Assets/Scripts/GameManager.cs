using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject menuCanvas;
    public static GameManager instance = null;

    // Game status bools
    public bool inGame, paused, destroyed, gameOver, restart;

    // Config bools
    public bool loadScenes;

    public int startLives;

    // Cross Game GUI Options
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
    public GUIText respawnTimerText;
    public GUIText livesText;
    private string gameOverMessage = "Game Over!";

    // Player attributes
    private int lives;
    private int score;

    public Transform startPosition;
    public float spawnInSpeed;

    // Making public to test spawn time;
    public int waitOnRespawn;
    private int countdown;

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
        activeShip = defaultShip;
        SetDefaults();
    }

    public void SetDefaults()
    {
        score = 0;
        lives = startLives;
        destroyed = false;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        respawnTimerText.text = "";
        livesText.text = "";
        scoreText.text = "";
        activeShip.transform.position = startPosition.position;
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

    public IEnumerator MoveIntoScene(System.Action<bool> finished)
    {

        Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);
        // Check if load scenes are enabled, if not send call back and break coroutine
        if (!loadScenes)
        {
            activeShip.transform.position = origin;
            inGame = true;
            finished(inGame);
            yield break;
        }
        
        Vector3 start = activeShip.transform.position;
        float step = (spawnInSpeed / (start - origin).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            activeShip.transform.position = Vector3.Lerp(start, origin, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        activeShip.transform.position = origin;
        inGame = true;
        finished(inGame);
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
            if (!destroyed)
            {
                activeShip.SetActive(false);
                selectedShip.SetActive(true);
            }
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
        Time.timeScale = 0;
        mainMenu.SetActive(true);
        paused = true;
        if (gameOver || destroyed)
        {
            // Little bit of logic to clean up UI when
            // open menu and "Game Over!"/Countdown timer message is on screen
            gameOverText.text = "";
            respawnTimerText.text = "";
        }
        //Disable scripts that still work while timescale is set to 0
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        // Disable all child panels in the main menu canvas (since we could be in children of children etc.)
        foreach (Transform transform in menuCanvas.transform)
        {
            transform.gameObject.SetActive(false);
        }
        paused = false;
        if (gameOver)
        {
            // If game is over, redisplay the game over text when closing
            // menu.
            gameOverText.text = gameOverMessage;
        }
        if (destroyed)
        {
            respawnTimerText.text = countdown.ToString();
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
        destroyed = true;
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
            countdown = waitOnRespawn;
            // Hacky way to countdown our coroutine to the player.
            while (countdown > 0)
            {
                respawnTimerText.text = countdown.ToString();
                yield return new WaitForSeconds(1.0f);
                countdown--;
            }
        }
        respawnTimerText.text = "";
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
