using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private AudioSource audioSource;
    public float speed;
    public float tilt;
    public Boundary boundary;
    
    public GameObject projectile;
    public Transform projectileSpawn;

    public float fireDelta;
    private float nextFire = 0.5F;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Executed before updated the frame, every frame
    private void Update()
    {
        // Only shoot if:
        // 1) The fire button is pressed
        // 2) Enough time has elapsed since last shot fired
        // 3) Time.timescale isn't 0 (i.e the game is not paused)
        // TODO: Time.timescale is GameController Logic - perhaps could pull paused info from there
        if (Input.GetButton("Fire1") && Time.time > nextFire && Time.timeScale != 0)
        {
            nextFire = Time.time + fireDelta;
            Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            audioSource.Play();
        }
    }

    // Called automatically by unity before each physiscs step
    public void FixedUpdate()
    {
        float moveHorizonatal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizonatal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        // Clamp player rb within the screen using current position and set boundaries
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
