using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    public float width = 1.0f;
    public LaserLight laserLight;
    public bool left = false;
    public AudioClip charging;
    public AudioClip firing;

    private Animator animator;
    private AudioSource source;

    public void OnLaserStart() {
        laserLight.active = true;
        //source.PlayOneShot(firing);
    }

    public void OnLaserStop() {
        laserLight.active = false;
    }

    public void OnAnimationEnd() {
        Destroy(gameObject);
    }

    public void PlaySound() {
        source.PlayOneShot(firing);
    }

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }
}
