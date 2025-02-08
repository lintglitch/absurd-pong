using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleImitator : MonoBehaviour {
    public GameObject paddle;
    // force which is used to do movements
    public float forceMovement = 30.0f;
    // how close he wants to get to the master paddle
    public float tolerance = 2.0f;

    protected Rigidbody2D body;
    protected Collider2D collider;
    protected AreaRestrictor restrictor;

    void FixedUpdate() {
        if(paddle == null) return;

        Vector2 diff = paddle.transform.position - transform.position;
        if(Mathf.Abs(diff.y) > tolerance) {
            body.AddForce(Vector2.up * Mathf.Sign(diff.y) * forceMovement);
        }
    }

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        restrictor = GetComponent<AreaRestrictor>();
    }
}
