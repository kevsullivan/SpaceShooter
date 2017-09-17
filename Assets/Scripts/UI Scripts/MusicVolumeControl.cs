using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeControl : MonoBehaviour {

    public Slider mySlider;

    public void Start()
    {
        mySlider.value = SoundManager.instance.musicSource.volume;
    }

    public void ChangeVolume()
    {
        SoundManager.instance.musicSource.volume = mySlider.value;
    }
}
