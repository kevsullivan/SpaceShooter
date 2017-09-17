using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // When loading a new level all objects in the scene are destroyed,
        // then the objects in the new level are loaded. In order to preserve an
        // object during level loading call DontDestroyOnLoad on it. If the object
        // is a component or game object then its entire transform hierarchy will not be destroyed either.
        DontDestroyOnLoad(gameObject);
	}

    public void Start()
    {
        efxSource.volume = 0.5f;
        musicSource.volume = 0.5f;
    }

    // Play a determined sound clip
    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }
	
    // Extensibility function (allows for ranomizing of sound clips played on triggers)
    public void RandomizeSfx(params AudioClip [] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();

    }
}
