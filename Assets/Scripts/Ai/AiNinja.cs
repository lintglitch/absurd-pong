using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiNinja : AiGeneral {
    // distance in x coordninates that AI will ignore
    public float IgnoreYDifference = 0.05f;
    // how much the ball speed will influence tracking, 0 for no influence
    public float BallYSpeedFactor = 0.0f;
    public float maxEngagementDistance = 10.0f;
    public Color invisibleColor;
    public float changeDuration;
    public Vector2 teleportLimit;
    public GameObject ballPrefab;

    // ball we are actively tracking
    protected GameObject focusedBall = null;

    private float changeCounter = 0.0f;
    private bool isInvisible = false;
    private SpriteRenderer sprite;
    private Color startColor;
    private float paddleForceCache;
    private bool specialMode = false;
    private float changeDurationCache;

    public override void OnInitialize() {
        sprite = paddle.GetComponent<SpriteRenderer>();
        startColor = sprite.color;
        paddleForceCache = paddle.forceMovement;
        changeDurationCache = changeDuration;
    }

    void FixedUpdate() {
        if(!active) return;

        // if we are
        if(changeCounter > 0) {
            HandleChanging();
            return;
        }

        if(UpdateEnvironment()) {
            //focusedBall
            GameObject[] allBallsIncoming = FilterIncomingBalls();
            // get ball closest to exit and stop it
            float x = 20.0f;
            foreach(GameObject ballObj in allBallsIncoming) {
                if(ballObj.transform.position.x < x) {
                    focusedBall = ballObj;
                    x = ballObj.transform.position.x;
                }
            }
        }

        bool notIdle = false;
        if(focusedBall) {
            Vector2 position = paddle.transform.position;
            Vector2 ballPosition = focusedBall.transform.position;
            Vector2 diff = ballPosition - position;
            Vector2 velocity = focusedBall.GetComponent<Rigidbody2D>().velocity;
            print("Ball velocity: " + velocity);


            if(ballPosition.x < -9.0f || ballPosition.x > 6.0f || velocity.x > 0.0f) notIdle = false;
            else if(isInvisible) {
                // find ideal teleport position
                bool limitReached = false;
                Vector2 teleportPos = ballPosition + new Vector2(-2.0f +velocity.x, velocity.y);
                print(teleportPos);

                if(teleportPos.x < teleportLimit.x) {
                    teleportPos.x = teleportLimit.x;
                    limitReached = true;
                }
                if(Mathf.Abs(teleportPos.y) > teleportLimit.y) {
                    teleportPos.y = teleportLimit.y * Mathf.Sign(teleportPos.y);
                    limitReached = true;
                }

                // if everything looks good and its still the default ball, lets try to change it
                Ball ball = focusedBall.GetComponent<Ball>();
                if(!limitReached && ball.type == "normal" && Mathf.Abs(velocity.y) < 3.2f) {
                    specialMode = true;
                    paddle.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                    paddle.forceMovement = 70.0f;
                    BeginChange();
                    GetComponent<AudioSource>().Play();
                    paddle.Teleport(teleportPos);
                    changeDuration = changeDuration * 3;
                    notIdle = true;
                }
                // normal teleport
                else {
                    BeginChange();
                    GetComponent<AudioSource>().Play();
                    paddle.Teleport(teleportPos);
                    notIdle = true;
                }
            }
            else {
                // not invisble and ball coming towards us, lets check if we should go invisible or not
                // ball too close, lets just try to get it
                if(diff.x <= 4.0f + Mathf.Abs(velocity.x) && diff.x > 1.0f) {
                    paddle.MoveY((diff.y + velocity.y*0.2f) * paddle.forceMovement);
                    notIdle = true;
                }
            }
        }

        // do some idle action
        if(notIdle == false) {
            // if we are currently not invisble, lets do that
            if(!isInvisible) {
                BeginChange();
            }
        }
    }

    public override void OnBallCollision(Ball ball) {
        // change balls into the prefab if its just a normal ball
        if(specialMode && ball.type == "normal") {
            ball.Replace(ballPrefab);
        }
    }

    private void HandleChanging() {
        changeCounter -= Time.deltaTime;

        if(isInvisible) {
            if(changeCounter <= 0) {
                sprite.color = startColor;
                isInvisible = false;
            }
            else {
                float percent = changeCounter / changeDuration;
                sprite.color = Color.Lerp(startColor, invisibleColor, percent);
            }
        }
        else {
            if(changeCounter <= 0) {
                sprite.color = invisibleColor;
                isInvisible = true;
                paddle.SetCollision(false);

                // restore previous settings
                if(specialMode) {
                    paddle.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    paddle.forceMovement = paddleForceCache;
                    specialMode = false;
                    changeDuration = changeDurationCache;
                }
            }
            else {
                float percent = changeCounter / changeDuration;
                sprite.color = Color.Lerp(invisibleColor, startColor, percent);
            }
        }
    }

    private void BeginChange() {
        Debug.Assert(changeCounter <= 0.0f);
        changeCounter = changeDuration;
        if(isInvisible) paddle.SetCollision(true);
    }
}
