using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HumanPlayer : Player {
    //protected PlayerInput input
    protected float yMovementInput;
    bool pressingFireButton = false;

    public override bool IsAI() {
        return false;
    }

    public void OnAction(InputValue value) {
        pressingFireButton = value.isPressed;
    }

    public void OnMove(InputValue value) {
        yMovementInput = value.Get<float>();
    }

    // tries triggering an action, will return true if successful
    public virtual bool TryTriggerAction() {
        return false;
    }

    public void TryMoving(float direction) {
        yMovementInput = direction;
    }

    // Update is called once per frame
    protected void FixedUpdate() {
        if(!IsActive()) return;

        if(pressingFireButton) {
            print("FIRING from " + gameObject.name);
            TryTriggerAction();
        }

        if(yMovementInput != 0.0f) {
            paddle.MoveYDirection(yMovementInput);
        }
    }
}
