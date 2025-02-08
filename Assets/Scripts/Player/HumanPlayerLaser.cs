using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlayerLaser : HumanPlayer {
    public GameObject prefab;
    public float xOffset;
    public float delay;
    public float backForce;

    private bool firing = false;
    private float fireTime;

    public override bool TryTriggerAction() {
        print("Try firing");
        if(!firing) {
            Fire(prefab);
            return true;
        }

        return false;
    }

    protected void Fire(GameObject bullet) {
        print("FIRING");
        firing = true;
        Quaternion rotation;
        float offset;
        if(player2) {
            rotation = Quaternion.identity;
            offset = xOffset;
        }
        else {
            rotation = Quaternion.Euler(0,0,180);
            offset = -xOffset;
        }
        GameObject blast = Instantiate(bullet, paddle.transform.position + new Vector3(offset, 0), rotation);
        blast.transform.parent = paddle.transform;
        blast.GetComponent<Laser>().left = !player2;
        fireTime = Time.time;
    }

    protected new void FixedUpdate() {
        if(!IsActive()) return;

        base.FixedUpdate();

        if(firing) {
            if(Time.time - fireTime > delay) firing = false;
            Vector2 direction = Vector2.right;
            if(player2) direction = Vector2.left;
            paddle.GetComponent<Rigidbody2D>().AddForce(direction * backForce);
            return;
        }
    }
}
