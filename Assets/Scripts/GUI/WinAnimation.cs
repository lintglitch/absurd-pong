using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinAnimation : MonoBehaviour {
    public GameControl controller;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI winnerStatusText;
    public GameObject panel;

    public Animator animation;
    public AudioClip winSound;
    public AudioClip looseSound;

    public void PlayAnimation(int winner, Player player1, Player player2) {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = winSound;

        winnerText.gameObject.SetActive(true);
        winnerStatusText.gameObject.SetActive(true);
        panel.SetActive(true);

        // player 2 won, set displayed texts accordingly
        if(winner == 1) {
            if(player2.IsAI()) {
                winnerStatusText.text = "LOOSE";
                audio.clip = looseSound;
            }
            else {
                winnerText.text = "Player 2";
                winnerText.fontSize = 1.1f;
            }
        }
        else if(!player2.IsAI()) {
            winnerText.text = "Player 1";
            winnerText.fontSize = 1.1f;
        }
        audio.Play();

        // play animation
        animation.Play("Win Animation");
    }

    public void OnWinEnd() {
        controller.EndGame();
    }
}
