using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerPaddle : MonoBehaviour {

    public Ball ball;
    public bool active = true;
    public bool xFollower = false;
    public float speed = 2.0f;
    public float minDistance = 5.0f;

    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 position = transform.position;
        Vector2 ballPosition = ball.transform.position;
        Vector2 distance = ballPosition - position;

        float vertDistance, horizDistance;
        if(xFollower) {
            vertDistance = distance.y;
            horizDistance = distance.x;
        }
        else {
            vertDistance = distance.x;
            horizDistance = distance.y;
        }

        // only react to changes if minimum distance is reached
        if(Mathf.Abs(vertDistance) < minDistance) {
            // go closer to the ball
            if(Mathf.Abs(horizDistance) > 0.1f) {
                Vector2 move;
                if(xFollower) move = new Vector2(horizDistance, 0);
                else move = new Vector2(0, horizDistance);

                body.velocity = move * speed;
            }
        }
        else {
            body.velocity = Vector2.zero;
        }

    }
}
