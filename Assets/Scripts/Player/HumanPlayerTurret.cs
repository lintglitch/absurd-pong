using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlayerTurret : HumanPlayer {
    public TurretAnimator[] turrets;
    public bool alternating = false;
    public float alternatingDelay;

    private int currentIndex = 0;
    private float previousTurretFired = 0;

    public override bool TryTriggerAction() {
        if (alternating) ShootAlternating();
        else ShootAll();
        return true;
    }

    private void ShootAlternating() {
        float time = Time.time;
        if (turrets[currentIndex].CanFire() && time > previousTurretFired + alternatingDelay) {
            turrets[currentIndex].Shoot();
            currentIndex++;
            if (currentIndex >= turrets.Length) currentIndex = 0;
            previousTurretFired = time;
        }
    }

    private void ShootAll() {
        foreach (var turret in turrets) {
            turret.Shoot();
        }
    }
}
