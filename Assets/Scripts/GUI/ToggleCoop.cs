using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleCoop : PersistentToggle {
    public Toggle versusToggle;

    public void Toggle() {
        bool playSound = true;

        // change
        if (gameObject.GetComponent<Toggle>().isOn) {
            if(versusToggle.isOn) {
                versusToggle.isOn = false;
                playSound = false;
            }
            MasterController.instance.coopActive = true; 
        }
        else {
            MasterController.instance.coopActive = false;
        }

        PlayerManagerController.instance.UpdateJoining();
        DoToggle(playSound);
    }

}
