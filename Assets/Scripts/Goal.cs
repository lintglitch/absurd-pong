using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public int player = 0;
    public GameControl gameControl;

    private void OnTriggerEnter2D (Collider2D other) {
        // increase score if its a ball
        if(other.tag == "Ball") {
            gameControl.ModifyPoints(player, 1);
            GetComponent<AudioSource>().Play();
            other.tag = "Clutter";

            if(player == 0 && other.GetComponent<Ball>().type == "rainbow" && UnlocksManager.instance != null) {
                UnlocksManager.instance.Unlock("RainbowBall");
            }
        }
    }
}
