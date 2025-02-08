using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShow : MonoBehaviour {
    public GameObject laserPrefab;
    public int laserAmount = 5;
    public float rotationSpeed = 1.0f;

    private GameObject[] lasers;

    // Start is called before the first frame update
    void Start() {
        lasers = new GameObject[laserAmount];
        Vector3 position = transform.position;
        float intervall = 180.0f / laserAmount;
        float currentAngle = 0.0f;
        for(int i=0; i<laserAmount; i++) {
            GameObject obj = Instantiate(laserPrefab, position, Quaternion.Euler(0, 0, currentAngle));
            obj.transform.parent = transform;
            lasers[i] = obj;
            currentAngle += intervall;
        }
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        // foreach(GameObject laser in lasers) {
        //     laser.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        // }
    }
}
