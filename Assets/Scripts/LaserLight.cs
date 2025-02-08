using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLight : MonoBehaviour {
    public bool active = false;
    public float force = 10.0f;
    public Laser laser;

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D other) {
        if(active) {
            float applyForce = force;
            string tag = other.tag;
            if(tag == "Paddle") {
                applyForce *= 2.0f;
            }

            Rigidbody2D body = other.GetComponent<Rigidbody2D>();
            if(body != null) {
                Vector2 direction = Vector2.right;
                if(laser.left) direction = Vector2.left;
                body.AddForce(direction * applyForce);
            }
        }
    }
}
