using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlayerLauncher : HumanPlayer {
    public GameObject prefab;
    public Vector2 spawnLocation;
    public float delay;

    protected float lastTriggerTime = 0.0f;

    public override bool TryTriggerAction() {
        if(Time.time - lastTriggerTime > delay) {
            lastTriggerTime = Time.time;
            Spawn();
            return true;
        }

        return false;
    }

    public void Spawn() {
        GetComponent<AudioSource>().Play();

        Vector2 spawnAt = new Vector2();
        Vector2 position = paddle.transform.position;

        if(player2) spawnAt.x = -spawnLocation.x;
        else spawnAt.x = spawnLocation.x;

        bool above;
        if(paddle.transform.position.y > 0) {
            spawnAt.y = position.y - spawnLocation.y;
            above = false;
        }
        else {
            spawnAt.y = position.y + spawnLocation.y;
            above = true;
        }

        GameObject obj = Instantiate(prefab, spawnAt, GetSpawnRoation(above));
    }

    protected Quaternion GetSpawnRoation(bool above = true) {
        float baseAngle;
        if(player2) baseAngle = -90.0f;
        else baseAngle = 90.0f;

        float modAngle = Random.Range(2.0f, 4.0f);
        if(!above) modAngle = -modAngle;
        if(player2) modAngle = -modAngle;

        return Quaternion.Euler(0, 0, baseAngle + modAngle);
    }
}
