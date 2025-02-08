using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    void Start() {
        // make permanent if created through a multiplayer match
        if(MasterController.instance != null && (MasterController.instance.versusActive || MasterController.instance.coopActive)) {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RemoveChildren() {
        for (var i = transform.childCount - 1; i >= 0; i--) {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    // player tries opening menu
    public void OnMenu(InputValue value) {
        GameObject obj = GameObject.FindWithTag("MenuCanvas");
        if(obj != null) {
            obj.GetComponent<OverlayMenu>().ToggleEscapeMenu();
        }
    }
}
