using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSoldier : AiSimple {
    public float defenceDistance = 7.0f;
    public GameObject bulletDefense;
    public float minBlastDistance = 5.0f;
    public float maxBlastDistance = 10.0f;
    public float maxBlastYValue = 3.0f;
    public float blastVelocityFactor = 1.0f;
    public float blastXOffset = 0.0f;
    public float firingDuration = 2.0f;
    public float IgnoreYDifferenceShoot;
    public float splitChance;
    public SpriteRenderer cannon;
    public bool useKillBullets;
    public bool useSplitBullets;

    protected GameObject incomingBall = null;
    protected GameObject otherBall = null;
    protected bool firing = false;
    protected float fireTime;
    private float extraFiringTime = 0.0f;

    void FixedUpdate() {
        if(!active) return;

        // can't do anything while firing
        if(firing) {
            if(Time.time - fireTime > firingDuration + extraFiringTime) {
                firing = false;
                extraFiringTime = 0.0f;
            }
        }

        if(UpdateEnvironment()) {
            // try getting the closest incoming ball we need to defend against
            incomingBall = null;
            otherBall = null;
            GameObject closest = GetNearestBall(FilterIncomingBalls());

            if(closest != null && closest.transform.position.x - paddle.transform.position.x < defenceDistance
                    && closest.transform.position.x > paddle.transform.position.x) {
                incomingBall = closest;
            }

            // no interesting ball, try getting some ball to blast, the best ball is the one we need to move the least for
            else {
                float yPos = paddle.transform.position.y;
                float minYDiff = 1000.0f;
                foreach(GameObject ballObj in savedBalls) {
                    float yDiff = Mathf.Abs(yPos - ballObj.transform.position.y);
                    if(yDiff < minYDiff) {
                        minYDiff = yDiff;
                        otherBall = ballObj;
                    }
                }
            }
        }


        bool notIdle = false;
        if(incomingBall) {
            notIdle = true;
            HandleBall(incomingBall);
        }
        else if(otherBall) {
            notIdle = true;
            Blast(otherBall, bulletDefense, useSplitBullets);
        }

        // do some idle action
        if(DoIdleActions && notIdle == false) {
            IdleAction();
        }
    }

    protected override bool HandleBall(GameObject ball) {
        Vector2 position = paddle.transform.position;
        Vector2 ballPosition = ball.transform.position;
        Vector2 diff = ballPosition - position;
        Vector2 velocity = ball.GetComponent<Rigidbody2D>().velocity;
        diff += new Vector2(0, velocity.y*BallYSpeedFactor);

        // ball behind us
        if(ballPosition.x < position.x) return false;

        // try to shoot ball
        if(!firing && diff.x > minBlastDistance && diff.x < maxBlastDistance) {
            Fire(bulletDefense, useKillBullets: useKillBullets);
        }

        // try to position yourself better, so the middle is pointing to the ball
        MoveSmoothly(ballPosition.y);
        return true;
    }

    protected void Blast(GameObject obj, GameObject bullet, bool trySplit = false) {
        Vector2 position = paddle.transform.position;
        Vector2 otherPosition = obj.transform.position;
        // Vector2 diff = otherPosition - position;
        Vector2 velocity = obj.GetComponent<Rigidbody2D>().velocity;

        float idealYPosition = otherPosition.y + velocity.y * blastVelocityFactor;
        idealYPosition = Mathf.Clamp(idealYPosition, -maxBlastYValue, +maxBlastYValue);

        if(firing || Mathf.Abs(idealYPosition - position.y) > IgnoreYDifferenceShoot) {
            MoveSmoothly(idealYPosition);
        }
        // in position
        if(!firing) {
            Fire(bullet, trySplit, useKillBullets: useKillBullets);
        }
    }

    protected void Fire(GameObject bullet, bool trySplit = false, bool useKillBullets = true) {
        firing = true;

        GameObject blast = Instantiate(bullet, paddle.transform.position + new Vector3(blastXOffset, 0), Quaternion.identity);
        Bullet bulletObj = blast.GetComponent<Bullet>();

        if(!useKillBullets) {
            bulletObj.killBullet = false;
        }

        if (trySplit) {
            bulletObj.killBullet = false;
            if(bulletObj.SetToSplit(splitChance)) {
                extraFiringTime = 1.0f;
            }
        }
        fireTime = Time.time;
    }

    public override void Hide() {
        base.Hide();
        cannon.enabled = false;
    }

    public override void Show() {
        base.Show();
        cannon.enabled = true;
    }

}
