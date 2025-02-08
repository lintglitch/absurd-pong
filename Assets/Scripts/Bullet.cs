using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public AudioClip start;
    public AudioClip impact;
    public float speed;
    public bool splitBullet;
    public bool killBullet = true;
     [Tooltip("Gives a random chance for split bullets. Should be between 0 and 1.")]
    public float randomSplitChance = 0;
    public Color split;
    public Color inactive;

    private Rigidbody2D body;
    private bool active = false;

    public void Kill() {
        if(!active) return;

        GetComponent<SpriteRenderer>().color = inactive;
        gameObject.tag = "Clutter";
        body.gravityScale = 1.0f;
        // make spend ammo not as impactful
        body.mass = body.mass / 5;
        this.enabled = false;
        active = false;

        // start disappearing
        DisappearDelay disappearDelay = GetComponent<DisappearDelay>();
        if (disappearDelay != null) disappearDelay.enabled = true;
    }

    public bool SetToSplit(float chance=1.0f) {
        if(chance >= 1.0f || Random.value < chance) {
            GetComponent<SpriteRenderer>().color = split;
            killBullet = false;
            splitBullet = true;
            GetComponent<AudioSource>().pitch = 0.65f;
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
        GetComponent<AudioSource>().PlayOneShot(start);
        if(killBullet == false && randomSplitChance > 0) SetToSplit(randomSplitChance);
        active = true;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(!active) return;

        GetComponent<AudioSource>().PlayOneShot(impact);

        string tag = other.gameObject.tag;
        if(tag == "Ball") {
            Ball ball = other.gameObject.GetComponent<Ball>();
            if(splitBullet) ball.Split();
            else if(killBullet) ball.Kill();
        }
        Kill();
    }

    // Update is called once per frame
    void FixedUpdate() {
        body.velocity = transform.right * speed;
    }
}
