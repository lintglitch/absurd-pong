using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSlowdown : AiSimple {

    public float slowdownStrength = 0.5f;
    public float slowdownDuration = 2.0f;
    public float slowdownTimeout = 5.0f;

    private float lastSlowdown = 0.0f;
    private bool slowdownActive = false;
    // public GameObject rocketPrefab;
    // public float delay;
    // // where things should spawn
    // public Vector2 spawnPosition;
    // public Blinking blinker;

    // private float counter;
    // private bool fireNextAbove = true;

    // protected override void IdleAction() {
    //     if(GoTowardsYCenter()) {
    //         counter = 0.0f;
    //     }
    //     else {
    //         // activate blinker if disabled
    //         if(!blinker.isActiveAndEnabled) {
    //             blinker.enabled = true;
    //         }

    //         // while in the middle start firing rockets
    //         counter += Time.deltaTime;
    //         if(counter >= delay) {
    //             counter = 0.0f;
    //             Vector2 spawnAt = new Vector3(spawnPosition.x, 0);
    //             if(fireNextAbove) spawnAt.y = spawnPosition.y;
    //             else spawnAt.y = -spawnPosition.y;


    //             float angleMod = Random.Range(2.0f, 10.0f);
    //             if(fireNextAbove) angleMod = -angleMod;
    //             GetComponent<AudioSource>().Play();
    //             GameObject rocketObj = Instantiate(rocketPrefab, spawnAt, Quaternion.Euler(0, 0, -90.0f + angleMod));
    //             Rocket rocket = rocketObj.GetComponent<Rocket>();
    //             fireNextAbove = !fireNextAbove;
    //         }
    //     }
    // }

    protected override bool HandleBall(GameObject ball) {
        Vector2 position = paddle.transform.position;
        Vector2 ballPosition = focusedBall.transform.position;
        Vector2 diff = ballPosition - position;
        Vector2 velocity = ball.GetComponent<Rigidbody2D>().velocity;

        if(BallYSpeedFactor != 0.0f) {
            diff += new Vector2(0, velocity.y*BallYSpeedFactor);
        }

        // ball behind us
        if(ballPosition.x < position.x) return false;

        // ball too far away
        if(diff.x > minimumDistance) return false;

            if(!slowdownActive && Time.time - lastSlowdown > slowdownTimeout) {
                StartCoroutine(SlowDownTime());
            }

        // if slowdown would be useful, see if we can make one
        // if(diff.x < 1.0f && (Mathf.Abs(velocity.y) > 5.0f || Mathf.Abs(diff.y) > 3.0f)) {
        //     if(!slowdownActive && Time.time - lastSlowdown > slowdownTimeout) {
        //         StartCoroutine(SlowDownTime());
        //     }
        // }


        // try to position yourself better, so the middle is pointing to the ball
        MoveSmoothly(ballPosition.y);
        return true;
    }

    IEnumerator SlowDownTime() {
        slowdownActive = true;
        GameControl.instance.ModTimeScale(-slowdownStrength);
        paddle.GetComponent<Rigidbody2D>().drag = 5.0f;
        paddle.forceMovement = 500.0f;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(slowdownDuration);

        paddle.GetComponent<Rigidbody2D>().drag = 15.0f;
        paddle.forceMovement = 300.0f;
        GameControl.instance.RestoreTimeScale();
        slowdownActive = false;
        lastSlowdown = Time.time;
        yield break;
    }
}
