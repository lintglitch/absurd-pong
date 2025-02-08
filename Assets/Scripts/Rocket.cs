using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    public bool active;
    public float force;
    public float randomTorque;
    public Sprite offGraphics;
    public Sprite onGraphics;
    public float duration;
    public float targetDuration;
    public GameObject target;
    public float forceTarget;

    public AudioClip startSound;
    public AudioClip impactSound;

    private Rigidbody2D body;
    private SpriteRenderer renderer;
    private float torque;
    private float counter = 0.0f;
    private float counterTarget = 0.0f;

    // Start is called before the first frame update
    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
        Activate();
    }

    void FixedUpdate() {
        if(active) {
            body.AddRelativeForce(Vector2.up * force);
            body.AddTorque(torque);

            counter += Time.deltaTime;
            if(counter > duration) Deactivate();
            else if(target != null && counterTarget < targetDuration) {
                // some target tracking
                counterTarget += Time.deltaTime;
                Vector2 diff = target.transform.position - transform.position;
                body.AddForce(diff.normalized * forceTarget);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        GameObject otherObj = other.gameObject;

        // make a ball reactivate a dead rocket or disable an active one
        if(otherObj.GetComponent<Ball>()) {
            if(active) Deactivate();
            else Activate();
            // ball already plays audio, so we don't have to
        }
        else if(otherObj.CompareTag("Paddle") || otherObj.CompareTag("Rocket")) {
            GetComponent<AudioSource>().PlayOneShot(impactSound);
        }
    }

    public void Activate() {
        active = true;
        renderer.sprite = onGraphics;
        counter = 0.0f;
        body.gravityScale = 0.0f;
        torque = Random.Range(-randomTorque, randomTorque);
        GetComponent<AudioSource>().PlayOneShot(startSound);
    }

    public void Deactivate() {
        active = false;
        renderer.sprite = offGraphics;
        counter = 0.0f;
        body.gravityScale = 1.0f;
    }
}
