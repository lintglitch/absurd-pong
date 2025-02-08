using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPongdorion : AiSimple {
    public SpriteRenderer glow;

    public GameObject[] spawnable;
    public float spawnDelay;
    public string spawnAnimation;
    public Vector2 spawnPosition;
    public float spawnDisappearDelay;

    private float counter;
    private int currentSpawnIndex = 0;
    private Player spawned = null;
    private Vector2 defaultGlowSize;

    public override void Hide() {
        base.Hide();
        glow.enabled = false;
    }

    public override void Show() {
        base.Show();
        glow.enabled = true;
    }

    public override void OnInitialize() {
        currentSpawnIndex = Random.Range(0, spawnable.Length);
        defaultGlowSize = glow.transform.localScale;
    }

    protected override void IdleAction() {
        if (GoTowardsYCenter()) {
            counter = 0.0f;
        }
        else {
            // while in the middle start firing rockets
            counter += Time.deltaTime;
            if (counter >= spawnDelay) {
                counter = 0.0f;

                // don't spawn another player if one is already spawned
                if (IsCurrentlyPlayerSpawned()) return;

                SpawnPlayer(currentSpawnIndex);
                gameObject.GetComponent<AudioSource>().Play();
                glow.gameObject.GetComponent<Animator>().Play("GlowShrinkage");
                //StartCoroutine(AuraEffect());
                currentSpawnIndex++;
                if (currentSpawnIndex >= spawnable.Length) currentSpawnIndex = 0;
            }
        }
    }

    public void SpawnPlayer(int spawnIndex) {
        GameObject prefab = spawnable[spawnIndex];

        Quaternion rotation = Quaternion.identity;
        if (prefab.GetComponent<Player>().player2Rotated) {
            rotation = Quaternion.Euler(0, 0, 180.0f);
        }

        GameObject playerObj = Instantiate(prefab, new Vector3(spawnPosition.x, spawnPosition.y), rotation);
        spawned = playerObj.GetComponent<Player>();
        spawned.player2 = true;
        
        DisappearDelay spawnedDisappearDealy = playerObj.AddComponent<DisappearDelay>();
        spawnedDisappearDealy.delay = spawnDisappearDelay;
        spawnedDisappearDealy.animationLength = 1.0f;
        spawnedDisappearDealy.sprite = playerObj.transform.Find("Paddle").GetComponent<SpriteRenderer>();

        // animate entrance
        spawned.startAnimation = spawnAnimation;
        spawned.StartAnimation();
    }

    public bool IsCurrentlyPlayerSpawned() {
        if (spawned == null) return false;
        return true;
    }

    IEnumerator AuraEffect() {
        float downScaleFactor = 2.0f;
        for(int i=1; i<11; i++) {
            float factor = 1.0f / ( (float) i);
            glow.transform.localScale = defaultGlowSize * factor;
            print(factor);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(5.0f);

    }
}
