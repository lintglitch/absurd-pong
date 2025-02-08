using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    // ball type
    public string type = "normal";
    // minimal speed, if the ball is slower it will accelerate
    public Vector2 minSpeed = new Vector2(2.0f, 0.0f);
    // max speed, if higher will reduce its speed, if 0 speed is unlimeted
    public float maxSpeed;
    // launch speed
    public float launchSpeed = 6.0f;
    public Vector2 minLaunchSpeed = new Vector2(3.0f, 0.0f);
    // force with which changes in velocity are applied
    public float forceChange = 10.0f;
    // how much the ball is affected by paddle velocity
    public float paddleVelocityFactor = 0.5f;
    // how much speed the ball gains by hitting the wall
    public float wallSpeedUpFactor = 0.1f;
    // should it launch itself from the very start
    public bool launchImmediately = false;
    // how often to check if the ball is stuck
    public float stuckCheckFrequency = 2.0f;
    public float stuckMinDistance = 0.3f;
    // if set will stop new balls from spawning (other then events)
    public bool preventSpawning = true;

    // audio clips
    public AudioClip impactWall;
    public AudioClip impactPaddle;
    public AudioClip impactClutter;
    public AudioClip impactBall;
    public AudioClip soundDies;

    // audio changes
    public Vector2 pitchRange = new Vector2(0.7f, 1.2f);
    public int pitchModifier = 3;
    public int volumeModifier = 24;

    private Rigidbody2D body;
    private AudioSource audioSource;
    // for determining if we stopped
    private Vector2 lastPosition;
    private float lastCheckTime = 0.0f;

    // applies velocity towards random direction
    [ContextMenu("Launch")]
    public void Launch(float factor=1.0f) {
        Vector2 direction = new Vector2(Random.Range(minLaunchSpeed.x, launchSpeed), Random.Range(minLaunchSpeed.y, launchSpeed));
        // ball should be able to go into both directions
        if(true) direction.x = -direction.x;
        //if(Random.value > 0.5f) direction.x = -direction.x;
        if(Random.value > 0.5f) direction.y = -direction.y;

        GetComponent<Rigidbody2D>().velocity = direction*factor;
    }

    public void Split() {
        GameObject newBall = Instantiate(gameObject, body.transform.position, Quaternion.identity);
    }

    // replaces the ball with a different prefab
    public void Replace(GameObject ballPrefab) {
        GameObject replacement = Instantiate(ballPrefab, body.transform.position, body.transform.rotation);
        Rigidbody2D rBody = replacement.GetComponent<Rigidbody2D>();
        rBody.velocity = body.velocity;
        Destroy(gameObject);
    }

    [ContextMenu("Reset")]
    public void Reset() {
        body.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        Launch();
    }

    [ContextMenu("Kill")]
    public void Kill() {
        audioSource.pitch = (pitchRange.x + pitchRange.y) / 2;
        audioSource.PlayOneShot(soundDies, 0.7f);
        gameObject.tag = "Clutter";
        body.sharedMaterial = null;
        body.gravityScale = 1.0f;
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if(trail != null) trail.enabled = false;
        this.enabled = false;
    }

    void Start() {
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
        lastCheckTime = Time.time;

        if(launchImmediately) Launch();
        //body.
    }

    void OnCollisionEnter2D(Collision2D other) {
        GameObject otherObj = other.gameObject;

        audioSource.volume = 1.0f;
        audioSource.pitch = 1.0f;

        string tag = otherObj.tag;
        if(tag == "Wall") {
            audioSource.clip = impactWall;
            audioSource.volume = getVolume();
            audioSource.pitch = getPitch();
            audioSource.PlayOneShot(impactWall);

            body.velocity += new Vector2(body.velocity.x * wallSpeedUpFactor, 0.0f);
        }
        else if(tag == "Paddle") {
            audioSource.clip = impactPaddle;
            audioSource.volume = getVolume();
            audioSource.pitch = getPitch();
            audioSource.PlayOneShot(impactPaddle);
        }
        else if(tag == "Clutter") {
            audioSource.volume = getVolume();
            audioSource.pitch = getPitch();
            audioSource.PlayOneShot(impactClutter);
        }
        else if(tag == "Ball" || tag == "Rocket" || tag == "Sphere") {
            audioSource.PlayOneShot(impactBall);
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        string tag = other.gameObject.tag;
        // alter speed when hitting a paddle

        if(tag == "Paddle") {
            Paddle paddle = other.gameObject.GetComponent<Paddle>();
            if(paddle == null) return;
            Rigidbody2D otherBody = paddle.GetComponent<Rigidbody2D>();
            body.velocity += otherBody.velocity * paddleVelocityFactor;
            //print("Increased velocity by " + otherBody.velocity);
        }
    }

    private float getVolume() {
        float volume = body.velocity.magnitude / volumeModifier;
        if(volume > 1.0f) volume = 1.0f;
        return volume;
    }

    private float getPitch() {
        float pitch = Mathf.Sqrt(body.velocity.magnitude) / pitchModifier;
        if(pitch > pitchRange.y) pitch = pitchRange.y;
        else if(pitch < pitchRange.x) pitch = pitchRange.x;
        //print("Wall " + (Mathf.Sqrt(body.velocity.magnitude) / 3));
        return pitch;
    }

    private bool didWeStop() {
        Vector2 diff = transform.position;
        diff -= lastPosition;
        if(diff.magnitude < stuckMinDistance) return true;
        return false;
    }

    void FixedUpdate() {
        if(Time.time-lastCheckTime > stuckCheckFrequency) {
            // check if we stayed at the same place for a long time
            if(didWeStop()) {
                print("Stuck! New ball!");
                Kill();
                return;
            }

            lastCheckTime = Time.time;
            lastPosition = transform.position;
        }

        // keep ball at a certain speed
        Vector2 newForce = Vector2.zero;
        Vector2 velocity = body.velocity;

        // keep x speed
        if(Mathf.Abs(velocity.x) < minSpeed.x) {
            newForce.x = forceChange * Mathf.Sign(velocity.x);
        }

        // keep y speed
        if(Mathf.Abs(velocity.y) < minSpeed.y) {
            // base y speed on y position instead if speed is too low
            float multi;
            if(Mathf.Abs(velocity.y) < 0.05f) multi = Mathf.Sign(-body.transform.position.y);
            else multi = Mathf.Sign(velocity.y);
            newForce.y = forceChange * multi;
        }

        body.AddForce(newForce);

        // reduce speed if too fast
        if(maxSpeed > 0 && velocity.magnitude > maxSpeed) {
            newForce = -velocity;
        }
    }
}
