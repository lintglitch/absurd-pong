using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandScaler : MonoBehaviour {
    public MusicAnalyzer analyzer;
    // band to use for scaling
    public int band = 0;
    // does object scale in both y and x
    public bool alsoScaleX = true;
    // max value for scaling
    public float maxScale;
    // scale when value is zero
    public float baseValue = 0.0f;
    // factor that is multiplied to the scale
    public float factor = 1.0f;

    private float scaleX;

    void Start() {
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update() {
        float value = analyzer.frequencyBands[band] * factor + baseValue;
        if(value > maxScale) value = maxScale;

        Vector3 scale = new Vector3(scaleX, value, 1);
        if(alsoScaleX) scale.x = value;

        transform.localScale = scale;
    }

}
