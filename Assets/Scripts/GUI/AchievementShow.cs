using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementShow : MonoBehaviour {
    public TextMeshProUGUI achievementTitle;
    public TextMeshProUGUI achievementText;

    public Animator animation;

    public void ShowAchievement(string title, string text) {
        achievementTitle.text = title;
        achievementText.text = text;
        GetComponent<AudioSource>().Play();
        animation.Play("AchievementAppear");
    }

    // Start is called before the first frame update
    void Start() {

    }
}
