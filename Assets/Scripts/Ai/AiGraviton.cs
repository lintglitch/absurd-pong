using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGraviton : AiSimple {
    public GameObject ballPrefab;
    public float changeChance = 1.0f;

    public override void OnBallCollision(Ball ball) {
        // change balls into the prefab if its just a normal ball
        if(ball.type == "normal" && Random.value < changeChance) {
            ball.Replace(ballPrefab);
        }
    }
}
