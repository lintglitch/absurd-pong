using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSpeedy : AiSimple {
    protected bool goingUp = true;

    protected override void IdleAction() {
        // move towards the middle
        Vector2 position = paddle.transform.position;
        if(goingUp) {
            if(position.y > 3.0f) goingUp = false;
            paddle.MoveYDirection(1.0f);
        }
        else {
            if(position.y < -3.0f) goingUp = true;
            paddle.MoveYDirection(-1.0f);
        }
    }
}
