using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;

[CreateAssetMenu(fileName = "Spawn", menuName = "ScriptableObjects/Random Spawn", order = 1)]
public class RandomSpawn : ScriptableObject {
    public string spawnName;
    public GameObject prefab;
    public int chance = 1;
    public string requiredUnlock;
    public bool spawnRandomAmount = false;
    public Vector2Int spawnRandomAmountValue = new Vector2Int(1, 3);
    public bool rotateWithSpawner = false;


    [Tooltip("Usees custom spawn code when set.")]
    public string executeCustomCode = "";

    public bool randomSpeed = true;
    public Vector2 randomSpeedX = new Vector2(0.5f, 1.2f);
    public Vector2 randomSpeedY = new Vector2(4.0f, 10.0f);
    
    public bool randomRotation = false;
    public float randomRotationAmount = 30.0f;

    public bool randomScaling = false;
    public bool randomScalingKeepRatio = false;
    public Vector2 randomScalingAmount = new Vector2(0.5f, 2.0f);

    public bool randomDirectionGravity = false;
    public bool directionDependentGravity = false;
    // will apply this gravity amount if either one of the above options is set
    public float gravityAmount = 1.0f;

    public bool IsUnlocked() {
        if (requiredUnlock == "") return true;

        // if no unlocks manager exists everything is simply unlocked
        if (UnlocksManager.instance == null) return true;

        return UnlocksManager.instance.IsUnlocked(requiredUnlock);
    }

    public int GetSpawnAmount() {
        if (!spawnRandomAmount) return 1;

        // always a significantly higher chance for only one thing
        if (Random.value > 0.7) return 1;

        return Random.Range(spawnRandomAmountValue.x, spawnRandomAmountValue.y + 1);
    }

    public float GetSpawnAngle(bool facingDown) {
        float rotation = 0;
        if (facingDown && rotateWithSpawner) rotation = 180.0f;
        if (randomRotation) rotation += Random.Range(-randomRotationAmount, randomRotationAmount);
        return rotation;
    }

    public bool ApplyRandomScaling(GameObject obj) {
        if (!randomScaling) return false;

        float randomX = Random.Range(randomScalingAmount.x, randomScalingAmount.y);
        float randomY;

        if (randomScalingKeepRatio) randomY = randomX;
        else randomY = Random.Range(randomScalingAmount.x, randomScalingAmount.y);

        obj.transform.localScale = new Vector3(obj.transform.localScale.x * randomX, obj.transform.localScale.x * randomY, 1.0f);
        return true;
    }

    public bool ApplyRandomSpeed(GameObject obj, bool facingDown) {
        if (!randomSpeed) return false;
        Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
        if (rigidbody == null) {
            Debug.LogWarning("RandomSpawn " + spawnName + " has random speed set but no rigidbody");
            return false;
        }

        Vector2 speed = Vector2.zero;
        speed.y = Random.Range(randomSpeedY.x, randomSpeedY.y);
        if (facingDown) speed.y = -speed.y;

        speed.x = Random.Range(randomSpeedX.x, randomSpeedX.y);
        if (Random.value > 0.5) speed.x = -speed.x;

        obj.GetComponent<Rigidbody2D>().velocity = speed;
        return true;
    }

    public bool ApplyGravity(GameObject obj, bool facingDown) {
        if (!randomDirectionGravity && !directionDependentGravity) return false;
        Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
        if (rigidbody == null) {
            Debug.LogWarning("RandomSpawn " + spawnName + " has gravity changes set but no rigidbody");
            return false;
        }

        float gravity = gravityAmount;
        if (randomDirectionGravity) {
            if (Random.value > 0.5f) gravity *= -1;
        }
        else if(directionDependentGravity) {
            if(facingDown) gravity *= -1;
        }

        rigidbody.gravityScale = gravity;
        return true;
    }
}
