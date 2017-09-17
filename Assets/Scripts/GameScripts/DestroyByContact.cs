using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;

    public AudioClip playerExplosionClip, hazardExplosionClip;

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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }
            

        if (explosion != null)
        {
            SoundManager.instance.PlaySingle(hazardExplosionClip);
            Instantiate(explosion, transform.position, transform.rotation);
        }

        // Special player explosion if colliding with player not bullet
        if (other.tag == "Player")
        {
            SoundManager.instance.PlaySingle(playerExplosionClip);
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
            

        gameController.AddScore(scoreValue);
        // Destroy what hit the game object
        Destroy(other.gameObject);
        // Destroy the game object itself (script attached to obstacles)
        Destroy(gameObject);
    }
}
