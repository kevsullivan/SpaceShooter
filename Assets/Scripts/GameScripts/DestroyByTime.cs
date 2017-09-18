using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    // How long the game object should live before being destroyed.
    // This script is explicitly attached as compoonent to objects spawned
    // during gameplay which need to be cleaned up (particle effects etc.)
    public float lifetime;
    private AudioSource audioSource; 

    private void Start()
    {
        // Play the sound for the Game object
        audioSource = GetComponent<AudioSource>();
        SoundManager.instance.PlayEffects(audioSource);
        // Destroy the object after a selected period of time
        Destroy(gameObject, lifetime);
    }
}
