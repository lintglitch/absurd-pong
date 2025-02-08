using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCommando : AiSimple {
    public GameObject rocketPrefab;
    public float delay;
    // where things should spawn
    public Vector2 spawnPosition;
    public Blinking blinker;

    private float counter;
    private bool fireNextAbove = true;

    protected override void IdleAction() {
        if(GoTowardsYCenter()) {
            counter = 0.0f;
        }
        else {
            // activate blinker if disabled
            if(!blinker.isActiveAndEnabled) {
                blinker.enabled = true;
            }

            // while in the middle start firing rockets
            counter += Time.deltaTime;
            if(counter >= delay) {
                counter = 0.0f;
                Vector2 spawnAt = new Vector3(spawnPosition.x, 0);
                if(fireNextAbove) spawnAt.y = spawnPosition.y;
                else spawnAt.y = -spawnPosition.y;


                float angleMod = Random.Range(2.0f, 10.0f);
                if(fireNextAbove) angleMod = -angleMod;
                GetComponent<AudioSource>().Play();
                GameObject rocketObj = Instantiate(rocketPrefab, spawnAt, Quaternion.Euler(0, 0, -90.0f + angleMod));
                Rocket rocket = rocketObj.GetComponent<Rocket>();
                rocket.randomTorque = 0.05f;
                fireNextAbove = !fireNextAbove;
            }
        }
    }

    protected override bool HandleBall(GameObject ball) {
        if(blinker.isActiveAndEnabled) {
            blinker.Reset();
            blinker.enabled = false;
        }

        return base.HandleBall(ball);
    }
}
