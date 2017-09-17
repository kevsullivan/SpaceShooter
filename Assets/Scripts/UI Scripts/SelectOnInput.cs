using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttondSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (buttondSelected == false && Input.GetAxisRaw("Vertical") != 0)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttondSelected = true;
        }
	}

    private void OnDisable()
    {
        buttondSelected = false;
    }
}
