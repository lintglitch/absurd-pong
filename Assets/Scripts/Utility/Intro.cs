using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Intro : MonoBehaviour {
    public GameObject musicController;
    public BandScaler[] bandScalers;
    public float factorIncrease;
    public float factorIncreaseDelay;

    public GameControl gameControl;

    // Start is called before the first frame update
    void Start() {
        gameControl = GameControl.instance;
    }

    // Update is called once per frame
    void Update() {
        Keyboard keyboard = Keyboard.current;
        if (keyboard.fKey.wasPressedThisFrame) {
            print("Pressed F");
            musicController.GetComponent<MusicController>().enabled = true;// SetActive(true);
            StartCoroutine(EnableMusicBarGradually());
        }

        if(keyboard.gKey.wasPressedThisFrame) {
            print("Pressed G");
            gameControl.SpawnIncident();
        }

        if(keyboard.hKey.wasPressedThisFrame) {
            print("Pressed H");
            gameControl.SpawnIncident("Clutter1");
        }
        if (keyboard.jKey.wasPressedThisFrame) {
            print("Pressed J");
            gameControl.SpawnIncident("BallRainbow");
        }

    }

    IEnumerator EnableMusicBarGradually() {
        yield return new WaitForSeconds(factorIncreaseDelay);
        for(int i=0; i<20; i++) {
            foreach(var scaler in bandScalers) {
                scaler.factor += factorIncrease;
            }
            yield return new WaitForSeconds(factorIncreaseDelay);
        }
    }
}
