using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSphere : MonoBehaviour {

    public float shootSalvoDelay = 2.0f;
    public float inbetweenShotsDelay = 0.5f;
    public ShootPattern pattern;
    public bool isAlive = true;
    public bool limitedTimeToLive = true;
    public float timeToLive = 10.0f;
    public float chanceForSplitBulletShooter = 0.2f;
    public AudioClip startSound;
    public AudioClip stopSound;

    public TurretAnimator TurretUp;
    public TurretAnimator TurretDown;
    public TurretAnimator TurretLeft;
    public TurretAnimator TurretRight;

    private float remainingSalvoDelay;
    private bool isFiring = false;
    private bool usedAgingDelay = false;

    // Start is called before the first frame update
    void Start() {
        remainingSalvoDelay = shootSalvoDelay;
        GetComponent<AudioSource>().PlayOneShot(startSound);

        if(Random.value < chanceForSplitBulletShooter) {
            // set all turrets to fire split bullets instead
            TurretUp.fireSplitBullets = true;
            TurretDown.fireSplitBullets = true;
            TurretLeft.fireSplitBullets = true;
            TurretRight.fireSplitBullets = true;
        }
    }

    void FixedUpdate() {
        if (!isAlive) return;

        if (limitedTimeToLive) {
            timeToLive -= Time.deltaTime;
            if (timeToLive <= 0 && !isFiring) Kill();
            else if(timeToLive <= 5.0f && !usedAgingDelay) {
                shootSalvoDelay += 1.0f;
                inbetweenShotsDelay += 1.0f;
                usedAgingDelay = true;
            }
        }

        if (!isFiring) {
            remainingSalvoDelay -= Time.deltaTime;

            if (remainingSalvoDelay <= 0) {
                ShootSalvo();
            }
        }
    }

    public void ShootSalvo() {
        if (isFiring) return;

        isFiring = true;
        remainingSalvoDelay = shootSalvoDelay;

        if (pattern == ShootPattern.Circular) StartCoroutine(ShootPatternCircular());
        else if (pattern == ShootPattern.Doubles) StartCoroutine(ShootPatternDoubles());
        else if (pattern == ShootPattern.All) StartCoroutine(ShootPatternAll());
        else if (pattern == ShootPattern.OnlyXAlternating) StartCoroutine(ShootPatternOnlyXAlternating());
        
    }

    public bool Kill() {
        if (!isAlive) return false;

        GetComponent<AudioSource>().PlayOneShot(stopSound);
        isAlive = false;
        GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        return true;
    }

    IEnumerator ShootPatternCircular() {
        TurretLeft.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        TurretUp.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        TurretRight.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        TurretDown.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        isFiring = false;
    }

    IEnumerator ShootPatternDoubles() {
        TurretLeft.Shoot();
        TurretRight.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        TurretUp.Shoot();
        TurretDown.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        isFiring = false;
    }

    IEnumerator ShootPatternAll() {
        TurretLeft.Shoot();
        TurretRight.Shoot();
        TurretUp.Shoot();
        TurretDown.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        isFiring = false;
    }

    IEnumerator ShootPatternOnlyXAlternating() {
        TurretLeft.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        TurretRight.Shoot();
        yield return new WaitForSeconds(inbetweenShotsDelay);
        isFiring = false;
    }


    [System.Serializable]
    public enum ShootPattern { Circular, Doubles, All, OnlyXAlternating }

}
