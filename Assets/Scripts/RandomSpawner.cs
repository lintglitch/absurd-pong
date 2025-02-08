using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class RandomSpawner : MonoBehaviour {
    public RandomSpawn[] randomSpawnsInput;

    // filtered list of valid random spawns
    private List<RandomSpawn> randomSpawns;
    private int totalSpawnsWeight;

    // Start is called before the first frame update
    void Start() {
        ParseSpawns();
    }

    public void Spawn(Vector3 spawnPosition, bool facingDown, string spawnId = "") {
        RandomSpawn chosen;
        if (spawnId == "") {
            chosen = GetRandomSpawn();
        }
        else {
            chosen = GetSpawnWithID(spawnId);
            if(chosen == null) {
                Debug.LogError("Could not find spawn with id " + spawnId);
                return;
            }
        }
        GenerateSpawn(chosen, spawnPosition, facingDown);
    }

    protected void GenerateSpawn(RandomSpawn spawn, Vector3 spawnPosition, bool facingDown) {
        int spawnAmount = spawn.GetSpawnAmount();
        for(int i=0; i<spawnAmount; i++) {
            GameObject spawned = Instantiate(spawn.prefab, spawnPosition, Quaternion.Euler(0, 0, spawn.GetSpawnAngle(facingDown)));

            spawn.ApplyRandomScaling(spawned);
            spawn.ApplyRandomSpeed(spawned, facingDown);
            spawn.ApplyGravity(spawned, facingDown);

            // execute custom code if set
            if(spawn.executeCustomCode != "") {
                ApplyCustomCode(spawn.executeCustomCode, spawn, spawned, facingDown);
            }
        }
    }

    protected RandomSpawn GetRandomSpawn() {
        int chosenIndex = Random.Range(0, totalSpawnsWeight + 1);

        int currentIndex = 0;
        foreach (var spawn in randomSpawns) {
            if (chosenIndex <= currentIndex + spawn.chance) {
                return spawn;
            }
            currentIndex += spawn.chance;
        }

        Debug.LogError("Did not return Random spawn");
        return null;
    }

    protected RandomSpawn GetSpawnWithID(string id) {
        foreach(var spawn in randomSpawns) {
            if (spawn.spawnName == id) return spawn;
        }
        return null;
    }

    private void ApplyCustomCode(string code, RandomSpawn spawn, GameObject spawned, bool facingDown) {

    }

    private void ParseSpawns() {
        randomSpawns = new List<RandomSpawn>();
        totalSpawnsWeight = 0;
        foreach (var spawn in randomSpawnsInput) {
            if (spawn.IsUnlocked()) {
                randomSpawns.Add(spawn);
                totalSpawnsWeight += spawn.chance;
            }
        }

        // print these chances
        string s = "Spawn chances:\n";
        foreach(var spawn in randomSpawns) {
            float chance = spawn.chance * 100;
            float spawnChance = chance / totalSpawnsWeight;
            s += string.Format(CultureInfo.InvariantCulture, "{0}: {1:f2}%\n", spawn.spawnName, spawnChance);
        }
        Debug.Log(s);
    }
}
