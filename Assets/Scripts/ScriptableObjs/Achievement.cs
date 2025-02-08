using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "ScriptableObjects/Achievement", order = 2)]
public class Achievement : ScriptableObject {
    public string id;
    public string name;
    public string description;
    public string finishedDescription = "";
    public GameObject target = null;
    public GameObject paddleNeeded = null;
    public bool unlocksNext = false;
    public bool defeatAllWithPaddle = false;

    public bool HasCompletedDescription() {
        if(finishedDescription != null && finishedDescription != "") {
            return true;
        }
        return false;
    }

    public bool DoesDefeatUnlock(string defeated, string paddleUsed) {
        // target must be set to be defeat unlockable
        if(target == null) return false;

        if(target.GetComponent<Player>().id != defeated) return false;
        if(paddleNeeded != null && paddleNeeded.GetComponent<Player>().id != paddleUsed) return false;
        return true;
    }
}
