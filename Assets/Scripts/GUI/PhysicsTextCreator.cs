using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
using TMPro;

public class PhysicsTextCreator : MonoBehaviour {
    public Texture2D textSprite;
    public GameObject physicsPixel;
    public float pixelDistance = 0.5f;
    //UnityEvent<GameObject> create

    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start() {
        startPosition = gameObject.transform.position;
        CreatePhysicsText();
    }

    public void CreatePhysicsText() {
        int width = textSprite.width;
        int height = textSprite.height;

        for(int x_index=0; x_index < width; x_index++) {
            for(int y_index=0; y_index < height; y_index++) {
                if(textSprite.GetPixel(x_index, y_index).a > 0.5f) {
                    Vector3 pos = new Vector3(startPosition.x + x_index * pixelDistance, startPosition.y + y_index * pixelDistance, 0);
                    GameObject obj = Instantiate(physicsPixel, pos, Quaternion.identity);
                    obj.transform.parent = gameObject.transform;
                    //print("Pixel " + x_index + " " + y_index);
                }
            }
        }
    }

}
