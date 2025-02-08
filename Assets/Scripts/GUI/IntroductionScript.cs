using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroductionScript : MonoBehaviour {
    public GameControl controller;
    public TextMeshProUGUI player1Name;
    public TextMeshProUGUI player2Name;
    public GameObject signVS;
    public GameObject signBoss;
    public AudioClip vsSound;
    public AudioClip bossSound;

    public void BeginAnimation(string player2, bool boss=false) {
        Animator animator = gameObject.GetComponent<Animator>();
        player2Name.text = player2;
        player2Name.gameObject.SetActive(true);

        if(boss) {
            GetComponent<AudioSource>().PlayOneShot(bossSound);
            signBoss.SetActive(true);
            //animator.SetBool("Boss", true);
            animator.Play("Boss");
        }
        else {
            GetComponent<AudioSource>().volume = 0.05f;
            GetComponent<AudioSource>().PlayOneShot(vsSound);
            player1Name.gameObject.SetActive(true);
            signVS.SetActive(true);
            //animator.SetBool("VSPlayer", true);
            animator.Play("VSPlayer");
        }
    }

    public void AnimatePlayer1() {
        controller.AnimatePlayer(0);
    }

    public void AnimatePlayer2() {
        controller.AnimatePlayer(1);
    }

    public void OnIntroductionEnd() {
        controller.StartGame();
    }

}
