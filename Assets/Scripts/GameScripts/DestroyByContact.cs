using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }
            

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        // Special player explosion if colliding with player not bullet
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            GameManager.instance.PlayerKilled();
        }
            
        // TODO: Player gets score if killed by object (do I want this mechanic?)
        GameManager.instance.AddScore(scoreValue);
        // Destroy what hit the game object
        Destroy(other.gameObject);
        // Destroy the game object itself (script attached to obstacles)
        Destroy(gameObject);
    }
}
