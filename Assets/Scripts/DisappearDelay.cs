using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearDelay : MonoBehaviour {
    // delay until object disappears in seconds
    public float delay = 10.0f;
    public float animationLength = 3.0f;
    public SpriteRenderer sprite;

    private float counter = 0.0f;
    private Color startColor;

    // Start is called before the first frame update
    void Start() {
        startColor = sprite.color;
    }

    // Update is called once per frame
    void Update() {
        counter += Time.deltaTime;
        if(counter > delay) {
            Destroy(gameObject);
        }
        else if(counter > delay - animationLength) {
            startColor.a = Mathf.Lerp(1.0f, 0.0f, (counter - (delay - animationLength))/animationLength  );
            sprite.color = startColor;
        }
    }
}
