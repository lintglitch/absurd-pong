using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPerfect : AiGeneral {
    public float minimumDistance = 4.0f;

    // ball we are actively tracking
    private GameObject focusedBall = null;

    void FixedUpdate() {
        if(!active) return;

        if(UpdateEnvironment()) {
            focusedBall = GetNearestBall(FilterIncomingBalls());
        }

        if(focusedBall) {
            HandleBall(focusedBall);
        }
    }

    // makes action to intercept incoming ball, returns true if action has been taken
    private bool HandleBall(GameObject ball) {
        Vector2 position = paddle.transform.position;
        Vector2 ballPosition = focusedBall.transform.position;
        Vector2 diff = ballPosition - position;

        // ball behind us
        if(ballPosition.x < position.x) return false;

        // ball too far away
        if(diff.x > minimumDistance) return false;

        Vector2 velocityBall = ball.GetComponent<Rigidbody2D>().velocity;

        // try to position yourself better, so the middle is pointing to the ball
        paddle.MoveY((diff.y + velocityBall.y*0.1f) * paddle.forceMovement);

        return false;
    }
}
