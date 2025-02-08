using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour {
    public float speedUp;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = speedUp;
    }
}
