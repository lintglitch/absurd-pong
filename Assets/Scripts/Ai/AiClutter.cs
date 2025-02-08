using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiClutter : AiSimple {
    public GameObject prefab;
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


                float angleMod = Random.Range(0.0f, 360.0f);
                if(fireNextAbove) angleMod = -angleMod;
                GetComponent<AudioSource>().Play();
                GameObject obj = Instantiate(prefab, spawnAt, Quaternion.Euler(0, 0, angleMod));
                ApplyRandomScaling(obj, 1.5f);
                Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
                if(body != null) {
                    float velocityY = 0.0f;
                    if(!fireNextAbove) velocityY = Random.Range(4.0f, 8.0f);
                    body.velocity = new Vector2(Random.Range(5.0f, 14.0f), velocityY);
                }

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

    private void ApplyRandomScaling(GameObject obj, float maxSize=2.0f, bool keepProportions=false) {
        float randomX = Random.Range(0.5f, maxSize);
        float randomY;

        if(keepProportions) randomY = randomX;
        else randomY = Random.Range(0.5f, maxSize);

        obj.transform.localScale = new Vector3(obj.transform.localScale.x * randomX, obj.transform.localScale.x * randomY, 1.0f);
    }
}
