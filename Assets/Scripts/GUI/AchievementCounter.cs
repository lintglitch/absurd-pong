using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementCounter : MonoBehaviour{
    public int updatePause = 50;

    private TextMeshProUGUI counterField;
    private int frames = 0;

    void Start() {
        counterField = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        // update this once in a while
        if (frames <= 0 && UnlocksManager.instance != null) {
            counterField.text = string.Format("{0}/{1}", UnlocksManager.instance.GetAchievedUnlocks(), UnlocksManager.instance.GetNumberPossibleUnlocks());
            frames = updatePause;
        }

        frames--;
    }
}

