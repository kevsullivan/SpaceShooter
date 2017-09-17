using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

    public float scrollSpeed;
    public float tileSizeZ;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        // Calculate a new position for background based on time -
        // function called each frame so scrolls to slightly differant position each frame
        // How much it scrolls == scrollSpeed
        // Loops the value t, so that it is never larger than length and never smaller than 0.
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);

        // Vector3.forward == (0, 0, 1)
        transform.position =  startPosition + Vector3.forward * newPosition;
	}
}
