using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DetectOutsideClick : MonoBehaviour {
    public string animatorActivationBool;
    public Camera camera;

    public bool triggerOnX = false;
    public Vector2 xlimits;

    public bool triggerOnY = false;
    public Vector2 ylimits;

    public UnityEvent outsideEvent;

    private Animator animator;

    void Start() {
        animator = camera.GetComponent<Animator>();
    }

    void Update() {
        Mouse mouse = Mouse.current;
        if(mouse.leftButton.isPressed && animator.GetBool(animatorActivationBool)) {
            Vector2 mousePos = camera.ScreenToWorldPoint(new Vector3(mouse.position.x.ReadValue(), mouse.position.y.ReadValue(), 0));
            if(!Within(mousePos)){
                outsideEvent.Invoke();
            }
        }
    }

    bool Within(Vector2 pos) {
        if(triggerOnX) {
            if(pos.x < xlimits.x || pos.x > xlimits.y) return false;
        }

        if(triggerOnY) {
            if(pos.y < ylimits.x || pos.y > ylimits.y) return false;
        }
        return true;
    }
}
