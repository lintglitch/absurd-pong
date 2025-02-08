using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaRestrictor : MonoBehaviour {

    public float forceMultiplier = 10.0f;
    public float minForce = 10.0f;
    public bool restrictX = false;
    public bool restrictY = false;

    // width and height of that restricted area, these tolerances go into either direction
    // a width of 1.0f means a total width of 2.0f (since it goes to +x and -x)
    public float width = 1.0f;
    public float height = 1.0f;

    // offset of the parent location
    public Vector2 offset = Vector2.zero;

    private Rigidbody2D body;
    private Transform parent;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        parent = transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector2 transformPosition = transform.position;
        Vector2 applyForce = Vector2.zero;

        if(restrictX) {
            float diff = parent.position.x - transformPosition.x + offset.x;

            if(diff < 0 && (diff + width < 0)) {
                applyForce.x = Mathf.Min((diff + width) * forceMultiplier, -minForce);
            }
            else if(diff > 0 && diff - width > 0) {
                applyForce.x = Mathf.Max((diff - width) * forceMultiplier, minForce);
            }
        }

        if(restrictY) {
            float diff = parent.position.y - transformPosition.y + offset.y;
            if(diff < 0 && diff + height < 0) {
                applyForce.y = Mathf.Min((diff + height) * forceMultiplier, -minForce);
            }
            else if(diff > 0 && diff - height > 0) {
                applyForce.y = Mathf.Max((diff - height) * forceMultiplier, minForce);
            }
        }

        body.AddForce(applyForce);
    }
}
