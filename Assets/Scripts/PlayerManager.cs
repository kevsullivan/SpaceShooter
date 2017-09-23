using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance = null;
    void Awake()
    {
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
}
