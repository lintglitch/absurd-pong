using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChangeSoundLevel : MonoBehaviour {
    public AudioMixer masterMixer;
    // group name of the mixer to change
    public string levelName;

    private Slider slider;

    void Start() {
        slider = gameObject.GetComponent<Slider>();
        float startVolume = PlayerPrefs.GetFloat(levelName, slider.value);
        masterMixer.SetFloat(levelName, startVolume);
        slider.value = startVolume;
    }

    public void SetVolume() {
        print("Slider " + levelName + " to value " + slider.value);
        masterMixer.SetFloat(levelName, slider.value);
        PlayerPrefs.SetFloat(levelName, slider.value);
    }
}
