using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Struct which will provide config in insepctor for spawn logic
// spawnObject: The object to spawn
// spawnProbability: The probability of spawning the spawnObject
//                   Note: Probabilities are 0.0 -> 1.0 and must add to 1.0
[System.Serializable]
public struct SpawnProbability
{
    public GameObject spawnObject;
    public float spawnProbability;
}

public class GameController : MonoBehaviour {
    
    // Wave spawning attributes: Ispector configurable
    public List<SpawnProbability> hazardsAndProbability;
    public Vector3 spawnValues;
    public int hazardCount;
    // Wave spawning attributes: Game controller logic
    private bool spawning;
    private float probability;
    private float cumlative_probability;

    // Spawnwait == How long to wait before looping to next spawn of Hazard
    public float spawnWait;
    // Startwait == How long to wait at start of Game before begining to spawn Hazards 
    // Gives player a warmup period
    public float startWait;
    // Wavewait == How long to wait between waves (gives the player a breather)
    // TODO: Can implement edits on wave behviour on each wave wait so next wave has increased difficulty
    public float waveWait;

    // Menu to display on pause
    private GameObject mainMenu;
    
    private void Start()
    {
        GameManager.instance.SpawnActiveShip();
        // Call back coroutine logic to load player into level before starting gameplay.
        StartCoroutine(GameManager.instance.MoveIntoScene(finished =>
        {
            if (finished)
            {
                StartCoroutine(LevelLogic());
            }
            else
            {
                Debug.Log("Got unexpected call back from coroutine");
            }
        }));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.HandleMenu();
        }

        if (GameManager.instance.restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(GameManager.instance.RestartLevel());
            }
        }
    }

    // Co-routine function for spawning waves of "hazard" (Asteroids)
    public IEnumerator LevelLogic()
    {
        yield return new WaitForSeconds(startWait);
        // Game is an infinite runner (keep spawning till dead)
        while(true){
            
            for (int i = 0; i < hazardCount; i++)
            {
                // Get random hazard from list of hazards
                // Roll a dice to get random probability (0.0 inclusive to 1.0 inclusive)
                probability = Random.value;
                cumlative_probability = 0.0f;
                spawning = true;
                
                // Got through hazards and if probability works out, spawn the selected GameObject
                for (int j = 0; j < hazardsAndProbability.Count && spawning; j++)
                {
                    cumlative_probability += hazardsAndProbability[j].spawnProbability;
                    if (probability < cumlative_probability)
                    {
                        GameObject spawn = hazardsAndProbability[j].spawnObject;

                        // Calculate a random spawn position
                        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                        // Instantiate the hazard object at the random spawn position we generated
                        // The quaternion identity corresponds to "no rotation" - the object is perfectly aligned with the world or parent axes
                        // I.e. our Asteroid (hazard) prefabs are already rotated as intended.
                        Instantiate(spawn, spawnPosition, Quaternion.identity);
                        spawning = false;
                    }
                }
                // Wait for next spawn
                yield return new WaitForSeconds(spawnWait);
            }
            // Wait a bit before starting a new wave
            yield return new WaitForSeconds(waveWait);
            if (GameManager.instance.gameOver)
            {
                GameManager.instance.AllowRestart();
                break;
            }
        }

    }

}
