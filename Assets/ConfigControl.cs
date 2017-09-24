using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigControl : MonoBehaviour {

    public Toggle configToggle;

	public void LoadScenesToggle()
    {
        GameManager.instance.loadScenes = configToggle.isOn;
    }
}
