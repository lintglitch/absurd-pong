using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockScript : MonoBehaviour {
    public string achievement;

    public void Unlock() {
        if(UnlocksManager.instance != null) {
            UnlocksManager.instance.Unlock(achievement);
        }
    }
}
