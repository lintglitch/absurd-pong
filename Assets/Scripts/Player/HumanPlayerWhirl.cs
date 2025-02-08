using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlayerWhirl : HumanPlayer {
    public float torqueForce;
    public Rigidbody2D body;

    // Update is called once per frame
    protected new void FixedUpdate() {
        if(!IsActive()) return;

        if(yMovementInput != 0.0f) {
            paddle.MoveYDirection(yMovementInput);
        }
    }
}
