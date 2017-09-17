using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;

    // Spawnwait == How long to wait before looping to next spawn of Hazard
    public float spawnWait;
    // Startwait == How long to wait at start of Game before begining to spawn Hazards 
    // Gives player a warmup period
    public float startWait;
    // Wavewait == How long to wait between waves (gives the player a breather)
    // TODO: Can implement edits on wave behviour on each wave wait so next wave has increased difficulty
    public float waveWait;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
    private int score;

    private bool gameOver, restart, paused;
    
    private void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        // TODO: Confirm need for instantiating int value (does it not default to 0?)
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(string.Format("Hit Escape {0}", paused));
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Only specifying the sceneName or sceneBuildIndex will load the scene with the Single mode
                SceneManager.LoadScene("SpaceShooter");
            }
        }
    }

    private void PauseGame()
    {
        Debug.Log("PAUSING");
        Time.timeScale = 0;
        paused = true;
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        Debug.Log("UN-PAUSING");
        Time.timeScale = 1;
        paused = false;
        //enable the scripts again
    }

    // Co-routine function for spawning waves of "hazard" (Asteroids)
    public IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        // Game is an infinite runner (keep spawning till dead)
        while(true){

            Debug.Log("Spawning Waves");
            for (int i = 0; i < hazardCount; i++)
            {
                // Get random hazard from list of hazards
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                // Calculate a random spawn position
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

                // Instantiate the hazard object at the random spawn position we generated
                // The quaternion identity corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes
                // I.e. our Asteroid (hazard) prefabs are already rotated as intended.
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (gameOver)
            {
                restartText.text = "Press 'R' to restart";
                restart = true;
                break;
            }
        }

    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
