using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsVolumeControl : MonoBehaviour {

    public Slider mySlider;

    public void Start()
    {
        mySlider.value = SoundManager.instance.efxSource.volume;
    }

    public void ChangeVolume()
    {
        SoundManager.instance.efxSource.volume = mySlider.value;
    }

}
