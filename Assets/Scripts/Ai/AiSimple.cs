using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSimple : AiGeneral {
    public float minimumDistance = 10.0f;
    public float maximumDistance = 0.0f;
    // how much the ball speed will influence tracking, 0 for no influence
    public float BallYSpeedFactor = 0.0f;
    // idle actions
    public bool DoIdleActions = true;

    // ball we are actively tracking
    protected GameObject focusedBall = null;

    void FixedUpdate() {
        if(!active) return;

        if(UpdateEnvironment()) {
            focusedBall = GetNearestBall(FilterIncomingBalls());
        }

        bool notIdle = false;
        if(focusedBall) {
            notIdle = HandleBall(focusedBall);
        }

        // do some idle action
        if(DoIdleActions && notIdle == false) {
            IdleAction();
        }
    }

    // makes action to intercept incoming ball, returns true if action has been taken
    protected virtual bool HandleBall(GameObject ball) {
        Vector2 position = paddle.transform.position;
        Vector2 ballPosition = ball.transform.position;
        Vector2 diff = ballPosition - position;

        if(BallYSpeedFactor != 0.0f) {
            Vector2 velocity = ball.GetComponent<Rigidbody2D>().velocity;
            diff += new Vector2(0, velocity.y*BallYSpeedFactor);
        }

        // ball behind us
        if(ballPosition.x < position.x) return false;

        // ball too far away
        if(diff.x > minimumDistance) return false;

        // ball too close
        if (maximumDistance > 0 && diff.x < maximumDistance) return false;

        // try to position yourself better, so the middle is pointing to the ball
        MoveSmoothly(ballPosition.y);
        return true;
    }

    protected virtual void IdleAction() {
        GoTowardsYCenter();
    }
}
