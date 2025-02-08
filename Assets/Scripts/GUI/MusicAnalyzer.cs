using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAnalyzer : MonoBehaviour {
    public float[] frequencyBands = new float[8];
    public float bandFallSpeed = 0.1f;

    // frequency samples
    private float[] samples = new float[512];
    // unbuffered frequency bands
    private float[] frequencyBandsData = new float[8];
    private AudioSource source;


    // Start is called before the first frame update
    void Start() {
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        UpdateSpectrumData();
        UpdateFrequencyBands();
    }

    void UpdateSpectrumData() {
        source.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        /*
        * Media has 44100 Hz
        * 44100 / 512 = 86
        */
        int currentBand = 0;
        for(int i=0; i < 8; i++) {
            // how many samples per band
            int sampleCount = (int) Mathf.Pow (2, i) * 2;

            // average of current samples
            float average = 0.0f;
            for(int j = 0; j < sampleCount; j++) {
                average += samples[currentBand] * (currentBand + 1);
                currentBand += 1;
            }

            average /= sampleCount;
            frequencyBandsData[i] = average * 10.0f;
        }
    }

    void UpdateFrequencyBands() {
        for(int i=0; i < 8; i++) {
            float target = frequencyBandsData[i];
            if(target > frequencyBands[i]) frequencyBands[i] = frequencyBands[i] = target;
            else frequencyBands[i] = Mathf.Lerp(frequencyBands[i], target, bandFallSpeed);
            //Mathf.MoveTowards(frequencyBands[i], target, 0.05f);
            // frequencyBands[i] = Mathf.Lerp(frequencyBands[i], target, 0.2f);
            //MoveTowardsfrequencyBandsData[i]
        }
    }
}
