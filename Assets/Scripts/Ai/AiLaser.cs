using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiLaser : AiGeneral {
    public float minBlastDistance = 5.0f;
    public float maxBlastDistance = 10.0f;
    public float blastYTolerance = 0.5f;
    public float maxBlastYValue = 3.0f;
    public float blastVelocityFactor = 1.0f;
    public float blastMaxXVelocity = 5.0f;
    public float blastXOffset = 0.0f;
    public float firingDuration = 2.0f;
    public float throwbackForce = 2.0f;
    public GameObject blastPrefab;

    // distance in x coordninates that AI will ignore
    public float IgnoreYDifference = 0.05f;
    // idle actions
    public bool DoIdleActions = true;

    protected GameObject focusedBall = null;
    protected bool firing = false;
    protected float fireTime;

    void FixedUpdate() {
        if(!active) return;

        // can't do anything while firing
        if(firing) {
            HandleFiring();
            return;
        }

        if(UpdateEnvironment()) {
            // try getting the closest incoming ball we need to defend against
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
    protected bool HandleBall(GameObject ball) {
        Vector2 position = paddle.transform.position;
        Vector2 ballPosition = ball.transform.position;
        Vector2 diff = ballPosition - position;
        Vector2 velocity = ball.GetComponent<Rigidbody2D>().velocity;

        // ball behind us
        if(ballPosition.x < position.x) return false;

        // ball far away, try to blast it
        if(!firing && diff.x > minBlastDistance && diff.x < maxBlastDistance && Mathf.Abs(velocity.x) < blastMaxXVelocity) {
            Blast(ball, blastPrefab);
            return true;
        }

        // try to position yourself better, so the middle is pointing to the ball
        if(Mathf.Abs(diff.y) > IgnoreYDifference) {
            paddle.MoveYDirection(1.0f * Mathf.Sign(diff.y));
            return true;
        }

        return false;
    }

    protected void IdleAction() {
        GoTowardsYCenter();
    }

    protected void Blast(GameObject obj, GameObject bullet) {
        Vector2 position = paddle.transform.position;
        Vector2 otherPosition = obj.transform.position;
        // Vector2 diff = otherPosition - position;
        Vector2 velocity = obj.GetComponent<Rigidbody2D>().velocity;

        float idealYPosition = otherPosition.y + velocity.y * blastVelocityFactor;
        idealYPosition = Mathf.Clamp(idealYPosition, -maxBlastYValue, +maxBlastYValue);

        if(Mathf.Abs(idealYPosition - position.y) > blastYTolerance) {
            paddle.MoveYDirection(1.0f * Mathf.Sign(idealYPosition - position.y));
        }
        // in position
        else {
            Fire(bullet);
        }
    }

    protected void Fire(GameObject bullet) {
        firing = true;
        GameObject blast = Instantiate(bullet, paddle.transform.position + new Vector3(blastXOffset, 0), Quaternion.identity);
        blast.transform.parent = paddle.transform;
        fireTime = Time.time;
    }

    protected void HandleFiring() {
        if(Time.time - fireTime > firingDuration) {
            firing = false;
            forceEnvironmentCheck = true;
        }
        paddle.GetComponent<Rigidbody2D>().AddForce(Vector2.left * throwbackForce);
    }
}
