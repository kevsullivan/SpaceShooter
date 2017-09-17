using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeControl : MonoBehaviour {

    public Slider mySlider;

    public void Start()
    {
        mySlider.value = AudioListener.volume;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = mySlider.value;
    }

}
