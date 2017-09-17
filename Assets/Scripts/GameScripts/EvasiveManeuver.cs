using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

    // vector2 with min/max wait time
    public Vector2 startWait;

    public Vector2 maneuverTime;
    public Vector2 maneuverWait;

    // Dodge helps pick the target maneuver
    public float dodge;
    // Smoothing value to garant control over evasion/maneuver speed
    public float smoothing;
    // Tilt  for entity during evasion
    public float tilt;
    // Track speed of rigid body component to ensure we can keep same speed during evasion
    private float currentSpeed;
    private float targetManeuver;
    private Rigidbody rb;
    public Boundary boundary;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
    }

    // Evading by setting target value along X axis and moving towards it over time
    // (i.e. needs to be co-routine, returns IENumerator)
    IEnumerator Evade()
    {
        // Wait a bit before doing anything (plays better)
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        // Loop to work through evasive maneuvers
        while (true)
        {
            // Need to set random target to maneuver to (point on X axis)
            // Reverse the sign (direction) of maneuver to ensure it always dodges towards
            // the origin (0,0,0 - i.e center) rather than possibly towards edge of game area
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            // Maneuver for a few seconds (random value)
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            // Set maneuver back to nothing (just fly forward like normal)
            targetManeuver = 0;
            // Wait a bit before maneuvering again
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

	void FixedUpdate ()
    {
        // This is essentially the same as Mathf.Lerp but instead the function will ensure that the speed never exceeds maxDelta.
        // Negative values of maxDelta pushes the value away from target.
        // So we move rigid bodies velocity in x direction towards target set in `Evade` and speed of maneuver
        // == time taken for frame to render * a smoothing value which allows control over speed from within Unity
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        // Update velocity (movement) of rb component towards new X at same speed the entity is currently travelling at
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);

        // Clamp entity rb within the screen using current position and set boundaries (insurance)
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        // Give maneuvering entity a tilt effect during evasion (similar to playercontroller behaviour)
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
