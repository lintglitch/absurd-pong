using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicAnimationsToggle : MonoBehaviour{
    public GameObject musicAnimationMenu;

    Toggle toggle;

    // Start is called before the first frame update
    void Start() {
        toggle = GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt("musicAnimation", 1) == 1;
    }

    public void SetValue() {
        int musicVal;
        if(toggle.isOn){
            musicVal = 1;
            musicAnimationMenu.SetActive(true);
        }
        else {
            musicVal = 0;
            musicAnimationMenu.SetActive(false);
        }
        PlayerPrefs.SetInt("musicAnimation", musicVal);
    }
}
