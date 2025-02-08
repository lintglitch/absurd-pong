using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAnimator : MonoBehaviour {
    public Turret turret;
    public bool fireSplitBullets = false;

    private Animator animator;

    public void Shoot() {
        animator.Play("Base Layer.TurretShoot");
    }

    public void OnShoot() {
        turret.TryFiring(fireSplitBullets);
    }

    //public void Shoot() {
    //    animator.
    //}

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public bool CanFire() {
        return turret.CanFire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
