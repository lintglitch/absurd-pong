using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiObstacle : AiGeneral {
    public float minimumDistance = 10.0f;
    public float IgnoreYDifference = 0.05f;

    // ball we are actively tracking
    private GameObject focusedBall = null;

    void FixedUpdate() {
        if(!active) return;

        if(UpdateEnvironment()) {
            focusedBall = GetNearestBall(savedBalls, false);
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

        // ball too far away
        if(Mathf.Abs(diff.x) > minimumDistance) return false;

        // try to position yourself better, so the middle is pointing to the ball
        if(Mathf.Abs(diff.y) > IgnoreYDifference) {
            paddle.MoveYDirection(1.0f * Mathf.Sign(diff.y));
            return true;
        }

        return false;
    }
}
