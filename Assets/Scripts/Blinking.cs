using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour {
    public Color onColor;
    public float onDuration;
    public float offDuration;
    public float switchDuration;

    private bool statusOn = false;
    private float counter = 0.0f;
    private Color offColor;
    private SpriteRenderer sprite;

    void Awake() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        offColor = sprite.color;
    }

    // Update is called once per frame
    void Update() {
        counter += Time.deltaTime;
        if(statusOn) {
            if(counter > onDuration) {
                statusOn = false;
                sprite.color = offColor;
                counter = 0.0f;
            }
            else if(counter > onDuration - switchDuration) {
                float percent = (counter - (onDuration - switchDuration))/switchDuration;
                sprite.color = Color.Lerp(onColor, offColor, percent);
            }
        }
        else {
            if(counter > offDuration) {
                statusOn = true;
                sprite.color = onColor;
                counter = 0.0f;
            }
            else if(counter > offDuration - switchDuration) {
                float percent = (counter - (offDuration - switchDuration))/switchDuration;
                sprite.color = Color.Lerp(offColor, onColor, percent);
            }
        }
    }

    public void Reset() {
        sprite.color = offColor;
        statusOn = false;
        counter = 0.0f;
    }
}
