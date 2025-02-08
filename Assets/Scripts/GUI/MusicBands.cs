using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBands : MonoBehaviour {
    public MusicAnalyzer analyzer;
    public Vector3 position;
    public GameObject bandPrefab;

    private GameObject[] bandObjects;
    private float scaleX;

    // Start is called before the first frame update
    void Start() {
        bandObjects = new GameObject[8];
        scaleX = bandPrefab.transform.localScale.x;
        Vector3 pos = position;
        for(int i=0; i<8; i++) {
            bandObjects[i] = Instantiate(bandPrefab, pos, Quaternion.identity);
            pos += Vector3.right * 2;
        }
    }

    // Update is called once per frame
    void Update(){
        for(int i=0; i<8; i++) {
            bandObjects[i].transform.localScale = new Vector3(scaleX, analyzer.frequencyBands[i], 1);
        }
    }
}
