using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGeneral : Player {
    // factor of speed of going back towards the center
    public float relaxedSpeedFactor = 0.5f;

    // minimum distance for us to actively follow the ball
    public float environmentCheckDelay = 1.0f;

    // balls in the cache
    protected GameObject[] savedBalls;
    protected bool forceEnvironmentCheck = false;

    // counter for the checkDelay
    private float delayCounter = 0.0f;

    public bool UpdateEnvironment() {
        delayCounter += Time.deltaTime;

        if(delayCounter > environmentCheckDelay || forceEnvironmentCheck) {
            UpdateBalls();
            delayCounter = 0.0f;
            forceEnvironmentCheck = false;
            return true;
        }
        return false;
    }

    public override bool IsAI() {
        return true;
    }

    // finds all balls on the field
    protected void UpdateBalls() {
        savedBalls = GameObject.FindGameObjectsWithTag("Ball");
    }

    // returns nearest ball
    protected GameObject GetNearestBall(GameObject[] balls, bool ignoreLostBalls=true) {
        Vector2 position = paddle.transform.position;
        GameObject nearestBallObj = null;
        Vector2 nearestPos = new Vector2();
        float nearestDistance = 0.0f;
        foreach(GameObject ballObj in balls) {
            float distance = Vector2.Distance(position, ballObj.transform.position);

            // ball is pretty close to us
            if(nearestBallObj == null || distance < nearestDistance) {
                // ignore it if it is behind us
                if(ignoreLostBalls && ballObj.transform.position.x < position.x) {
                    continue;
                }

                nearestBallObj = ballObj;
                nearestPos = ballObj.transform.position;
                nearestDistance = distance;
            }
        }
        return nearestBallObj;
    }

    // returns a list of balls that are actually coming towards us
    protected GameObject[] FilterIncomingBalls() {
        List<GameObject> incoming = new List<GameObject>();
        float xPosition = paddle.transform.position.x;

        foreach(GameObject ball in savedBalls) {
            float xBall = ball.transform.position.x;

            // only balls that go towards us
            Vector2 velocity = ball.GetComponent<Rigidbody2D>().velocity;
            if(velocity.x > 0) continue;
            incoming.Add(ball);
        }
        return incoming.ToArray();
    }

    // moves the paddle closer to the center of the screen, will return true if it moved
    protected bool GoTowardsYCenter() {
        // move towards the middle
        Vector2 position = paddle.transform.position;
        if(position.y > 0.5f) {
            paddle.MoveYDirection(-relaxedSpeedFactor);
            return true;
        }
        else if(position.y < -0.5f) {
            paddle.MoveYDirection(relaxedSpeedFactor);
            return true;
        }
        return false;
    }

    protected void MoveSmoothly(float yPosition) {
        float diff = yPosition - paddle.transform.position.y;
        float amount = Mathf.Sign(diff) * Mathf.Min(Mathf.Abs(diff), 1.0f) * paddle.forceMovement;
        //paddle.MoveYDirection(1.0f * Mathf.Sign(idealYPosition - position.y));
        paddle.MoveY(amount);
    }
}
