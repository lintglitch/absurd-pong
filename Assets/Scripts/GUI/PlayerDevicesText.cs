using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDevicesText : MonoBehaviour {
    public TextMeshProUGUI counterField;
    public int updatePause = 50;
    [TextArea] public string helpMessage;
    
    private int frames = 0;

    void Update() {
        // update this once in a while
        if (frames <= 0) {
            frames = updatePause;

            if (PlayerManagerController.instance == null || !MasterController.instance.IsMultiplayer()) {
                counterField.text = "";
                return;
            }

            // display help message if versus or coop was selected but no players joined yet
            if(PlayerManagerController.instance.GetNumberOfPlayers() == 0) {
                counterField.text = helpMessage;
                return;
            }

            counterField.text = PlayerManagerController.instance.GeneratePlayerInputsString();
        }

        frames--;
    }
}
