using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFollowPlayer : MonoBehaviour {
    public Paddle paddle;
    public float delayBeforeAssignment = 1.0f;
    public float forceMultiplier = 10;
    
    private Player followPlayer;

    public void AssignRandomPlayer() {
        if (GameControl.instance == null) return;

        // somewhat lower chance to follow the other player, since its more fun if it follow the human
        if (Random.value > 0.8) followPlayer = GameControl.instance.GetPlayer2();
        else followPlayer = GameControl.instance.GetPlayer1();
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(DelayedAssignPlayer());
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (followPlayer == null) return;

        float dy = followPlayer.GetPaddlePosition().y - paddle.transform.position.y;

        paddle.MoveY(dy * forceMultiplier);
    }

    IEnumerator DelayedAssignPlayer() {
        yield return new WaitForSeconds(delayBeforeAssignment);
        AssignRandomPlayer();
    }
}
