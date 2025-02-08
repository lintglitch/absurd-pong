using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incident : MonoBehaviour {
    public AudioClip appearSound;
    public AudioClip createSound;
    public AudioClip awaySound;

    private AudioSource audio;
    private Animator animator;
    private RandomSpawner spawner;
    private Vector2 spawnPosition;
    private bool pointingDownwards;
    private string spawnNext = "";

    // Start is called before the first frame update
    void Start() {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spawner = GetComponent<RandomSpawner>();
    }

    public void CreateIncident(Vector2 position, bool flipped, string spawnID="") {
        gameObject.transform.position = position;
        spawnPosition = position;
        pointingDownwards = flipped;
        spawnNext = spawnID;

        if (flipped) {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 180.0f);
        }
        else {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0.0f);
        }

        audio.clip = appearSound;
        audio.Play();
        animator.Play("Appear");
    }

    public void OnSpawn() {
        audio.clip = createSound;
        audio.Play();

        spawner.Spawn(spawnPosition, pointingDownwards, spawnNext);
    }

    public void OnDisappear() {
        audio.clip = awaySound;
        audio.Play();
    }
}
