using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectorGuiCounter : MonoBehaviour {
    public Selector choices;
    public int updatePause = 50;

    private TextMeshProUGUI counterField;
    private int frames = 0;

    void Start() {
        counterField = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        // update this once in a while
        if(frames <= 0) {
            counterField.text = string.Format("{0}/{1}", choices.GetTotalUnlockedChoices()-1, choices.GetTotalChoices()-1);
            frames = updatePause;
        }

        frames--;
    }
}
