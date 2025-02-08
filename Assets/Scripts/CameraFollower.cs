using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {
    public Transform follow;
    public bool followX;
    public bool followY;

    public bool xLimitActive = false;
    public Vector2 xLimit;

    public bool yLimitActive = false;
    public Vector2 yLimit;

    // Update is called once per frame
    void Update() {
        if (follow == null) return;
        if (followX) CopyX(follow.position);
        if (followY) CopyY(follow.position);
    }

    private void CopyX(Vector3 newPos) {
        Vector3 position = gameObject.transform.position;
        float val = newPos.x;

        if (xLimitActive) {
            if (val < xLimit.x) {
                gameObject.transform.position = new Vector3(xLimit.x, position.y, position.z);
                return;
            }
            if (val > xLimit.y) {
                gameObject.transform.position = new Vector3(xLimit.y, position.y, position.z);
                return;
            }
        }

        gameObject.transform.position = new Vector3(val, position.y, position.z);
    }

    private void CopyY(Vector3 newPos) {
        Vector3 position = gameObject.transform.position;
        float val = newPos.y;

        if(yLimitActive) {
            if (val < yLimit.x) {
                gameObject.transform.position = new Vector3(position.x, yLimit.x, position.z);
                return;
            }
            if (val > yLimit.y) {
                gameObject.transform.position = new Vector3(position.x, yLimit.y, position.z);
                return;
            }
        }

        gameObject.transform.position = new Vector3(position.x, val, position.z);
    }
}
