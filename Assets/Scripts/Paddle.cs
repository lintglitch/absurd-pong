using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    // force which is used to do movements
    public float forceMovement = 30.0f;

    protected Rigidbody2D body;
    protected Collider2D collider;
    protected AreaRestrictor restrictor;
    protected bool active = true;

    private ContactFilter2D filter;
    private Player player;

    public void SetPlayer(Player p) {
        player = p;
    }

    public void SetActive(bool isActive) {
        active = isActive;
    }

    public bool IsActive() {
        return active;
    }

    public void SetCollision(bool collisionActive) {
        collider.enabled = collisionActive;
    }

    public void MoveYDirection(float direction) {
        MoveY(forceMovement * direction);
    }

    // moves paddle, should only be called from fixed update
    public void MoveY(float amount) {
        if(!active) return;

        // only allow movement in the direction if there is enough space (stops bouncing)
        RaycastHit2D[] rayCastResults = new RaycastHit2D[1];
        int hits = collider.Raycast(Vector2.up * Mathf.Sign(amount), filter, rayCastResults, collider.bounds.extents.y + 0.05f);
        if(hits == 0)
            body.AddForce(Vector2.up * amount);
        else
            body.velocity = Vector2.zero;
    }

    public void MoveX(float amount) {
        if(!active) return;
        body.AddForce(Vector2.right * amount);
    }

    public void Teleport(Vector2 position) {
        body.velocity = Vector2.zero;
        body.position = position;
        restrictor.offset = position;
        print(position);
    }

    protected void OnCollisionEnter2D(Collision2D other) {
        GameObject otherObj = other.gameObject;
        if(otherObj.CompareTag("Ball")) {
            if(player != null) player.OnBallCollision(otherObj.gameObject.GetComponent<Ball>());
        }
    }

    void Start() {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        restrictor = GetComponent<AreaRestrictor>();

        filter = new ContactFilter2D();
        filter.useTriggers = false;
    }
}
