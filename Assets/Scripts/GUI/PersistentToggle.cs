using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistentToggle : MonoBehaviour {
    public string optionName;

    void Start() {
        Initialize();
    }

    protected void Initialize() {
        print("Initialize " + optionName + "value " + PlayerPrefs.GetInt(optionName, 0));
        // load saved toggle state without playing a sound
        if (PlayerPrefs.GetInt(optionName, 0) != 0) {
            print("Activating it");
            // overwrite audio activation to avoid beep at start of game
            AudioSource source = gameObject.GetComponent<AudioSource>();
            source.enabled = false;
            gameObject.GetComponent<Toggle>().isOn = true;
            source.enabled = true;
        }
    }

    protected void DoToggle(bool playSound = true) {
        AudioSource source = GetComponent<AudioSource>();
        if (playSound && source.enabled) source.Play();

        Toggle toggle = gameObject.GetComponent<Toggle>();
        if (toggle.isOn) {
            PlayerPrefs.SetInt(optionName, 1);
        }
        else {
            PlayerPrefs.SetInt(optionName, 0);
        }
    }
}