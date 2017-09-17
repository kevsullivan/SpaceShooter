using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    // How long the game object should live before being destroyed.
    // This script is explicitly attached as compoonent to objects spawned
    // during gameplay which need to be cleaned up (particle effects etc.)
    public float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
