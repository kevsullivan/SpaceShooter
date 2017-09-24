using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public bool isPickup;
    public int scoreValue;
    public int lifeValue;
    
    void OnTriggerEnter(Collider other)
    {
        // TODO: Smart logic for this problem of not destroying things under certain circumstances
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Pickup") || (isPickup && other.tag != "Player"))
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
            if (!isPickup)
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                GameManager.instance.PlayerKilled();
            }
        }
        else
        {
            // Destroy what hit the game object
            Destroy(other.gameObject);
        }
         
        // Calculate attribute modification
        if (scoreValue != 0)
        {
            // TODO: Player gets score if killed by object (do I want this mechanic?)
            GameManager.instance.AddScore(scoreValue);
        }
        if (lifeValue != 0)
        {
            // TODO: Player gets score if killed by object (do I want this mechanic?)
            GameManager.instance.AddLife(lifeValue);
        }
        // Destroy the game object itself (script attached to obstacles)
        Destroy(gameObject);
    }
}
