using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public string id = "Player";
    public string name = "Player";
    public bool active = true;
    public bool player2;
    public bool player2Rotated = true;
    public string startAnimation = "PlayerDefault";
    public bool bossIntroduction = false;
    // short string about the power level, for now only shown in descriptor for AIs, often a number
    public string powerLevelDescription;
    // if set an additional achievement is required for it to appear (either as an enemy AI or player choice)
    public string achievementRequired = "";
    // description as shown in game within the selector
    [TextArea(4,10)] public string description;
    // if given will override the normal music
    public AudioClip themeSong;

    protected Paddle paddle;
    protected Animator animator;

    void Start() {
        paddle.SetPlayer(this);

        OnInitialize();
    }

    public void StartAnimation() {
        if (animator != null) {
            print("Animate " + name);
            animator.Play(startAnimation);
        }
    }

    public void OnAnimationEnd() {
        if(animator != null) {
            animator.enabled = false;
        }

        // reset rotation, because it bugs out sometimes
        paddle.transform.rotation = transform.rotation;
        print("Player " + name + " has started");
        //paddle.SetAreaRestrictor(transform.position);
    }

    public virtual void Hide() {
        paddle.GetComponent<SpriteRenderer>().enabled = false;
    }

    public virtual void Show() {
        paddle.GetComponent<SpriteRenderer>().enabled = true;
    }

    public virtual void OnInitialize() {
        return;
    }

    public virtual void OnBallCollision(Ball ball) {
        return;
    }

    public bool IsActive() {
        return paddle != null && active;
    }

    public float GetDistanceFromStart() {
        return (paddle.transform.position - transform.position).magnitude;
    }

    public virtual bool IsAI() {
        return false;
    }

    public Vector2 GetPaddlePosition() {
        return paddle.transform.position;
    }

    void Awake() {
        Transform obj = transform.Find("Paddle");
        paddle = obj.GetComponent<Paddle>();
        animator = gameObject.GetComponent<Animator>();
    }
}
