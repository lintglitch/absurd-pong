using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public GameObject bullet;
    public Vector3 fireOffset;
    public float shootDelay;
    public bool autoShoot = false;

    // recoil
    public float recoilForce = 0;
    public Rigidbody2D mainBody;

    private float lastShootTime;
    private int counter = 0;

    // Start is called before the first frame update
    void Start() {
       lastShootTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if(autoShoot) {
            TryFiring();
        }
    }

    public bool TryFiring(bool forceSplitBullet = false) {
        if(CanFire()) {
            Fire(bullet, forceSplitBullet);
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    public bool CanFire() {
        if (Time.time - lastShootTime > shootDelay) {
            return true;
        }
        return false;
    }

    private void Fire(GameObject bullet, bool splitBullet=false, bool killBullet=false) {
        // firing = true;
        Vector3 relativeOffset = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * fireOffset;
        GameObject blast = Instantiate(bullet, transform.position + relativeOffset, transform.rotation);
        
        if(killBullet) {
            blast.GetComponent<Bullet>().killBullet = true;
        }
        else if(splitBullet) {
            blast.GetComponent<Bullet>().SetToSplit();
        }
        else {
            blast.GetComponent<Bullet>().killBullet = false;
        }

        // apply recoil if set
        if(recoilForce != 0 && mainBody != null) {
            mainBody.AddRelativeForce(-transform.right * recoilForce);
        }
    }
}
