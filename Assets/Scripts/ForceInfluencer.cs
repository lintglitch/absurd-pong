using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceInfluencer : MonoBehaviour {
    public float force = 10.0f;
    public Vector2 direction;
    public bool randomYDirection = false;

    protected Rigidbody2D body;

    // Start is called before the first frame update
    void Awake() {
        body = GetComponent<Rigidbody2D>();

        if(randomYDirection) {
            if(Random.value > 0.5f) direction = Vector2.up;
            else direction = Vector2.down;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        body.AddForce(direction * force);
    }
}
