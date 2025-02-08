using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    public AudioClip[] normalMusic;
    public AudioClip[] bossMusic;
    public GameObject musicBars;
    public bool autoStart = false;

    void Start() {
        // check if music animations are on
        bool musicAnimation = PlayerPrefs.GetInt("musicAnimation", 1) != 0;
        // if they are on but music is off then don't display them
        if(PlayerPrefs.GetFloat("MusicVolume", -10.0f) <= -60) musicAnimation = false;


        if(musicAnimation) {
            gameObject.GetComponent<MusicAnalyzer>().enabled = true;
            musicBars.SetActive(true);
        }

        else {
            gameObject.GetComponent<MusicAnalyzer>().enabled = false;
            musicBars.SetActive(false);
        }

        if (autoStart) PlayMusic(false);
    }

    public void PlayMusic(bool boss, AudioClip overrideMusic=null) {
        AudioClip music;

        // if override Music is given then use that one instead
        if(overrideMusic != null) {
            music = overrideMusic;
        }
        // choose music
        else {
            if (boss) {
                music = bossMusic[Random.Range(0, bossMusic.Length)];
            }
            else {
                music = normalMusic[Random.Range(0, normalMusic.Length)];
            }
        }

        AudioSource source = GetComponent<AudioSource>();
        source.clip = music;
        source.Play();
    }

}
