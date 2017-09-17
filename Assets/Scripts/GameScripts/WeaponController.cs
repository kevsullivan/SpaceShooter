using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    private AudioSource audioSource;

    public GameObject shot;
    // Using transform over game object for shotspwan to avoid code calls like `shotSpawn.transform.position`
    // Instead in unity when we drag the shotSpawn gameobject - unity will find transform component
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();

        // Invokes the method methodName in time seconds, then repeatedly every repeatRate seconds.
        // TODO: Maybe add smarter fireRate (realism :S) -> could do random range
        InvokeRepeating("Fire", delay, fireRate);
	}

    void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        audioSource.Play();
    }
}
