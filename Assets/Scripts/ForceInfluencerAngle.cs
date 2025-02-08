using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceInfluencerAngle : MonoBehaviour {
    public float force = 10.0f;
    public bool clockWise = true;

    protected Rigidbody2D body;

    // Start is called before the first frame update
    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        float effectiveForce;
        if (clockWise) effectiveForce = -force;
        else effectiveForce = force;
        body.AddTorque(effectiveForce);
    }
}
