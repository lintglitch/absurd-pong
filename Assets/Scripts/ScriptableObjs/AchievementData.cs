using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;

[CreateAssetMenu(fileName = "List", menuName = "ScriptableObjects/Achievement Data", order = 3)]
public class AchievementData : ScriptableObject {
    [Reorderable]
    public AchievementList achievements;

    private Dictionary<string, Achievement> buffer = new Dictionary<string, Achievement>();

    void OnEnable() {
        // iterate through the achievements and buffer them in a nice to access dictionary
        foreach(Achievement a in achievements) {
            string id = a.id;

            // if id is not set just use the objects id
            if(id == null || id == "") {
                Debug.LogError("Achievement has no id. Name: " + a.name);
                continue;
            }

            if(buffer.ContainsKey(id)) {
                Debug.LogError("Achievement buffer already contains key: " + id);
                continue;
            }

            buffer[id] = a;
        }
    }

    public Achievement GetAchievement(string id) {
        foreach(string key in buffer.Keys) {
            Debug.Log(key);
        }

        if(!buffer.ContainsKey(id)) {
            Debug.LogError("Tried accessing non-existing achievement id:" + id);
            return null;
        }

        return buffer[id];
    }

    [System.Serializable]
    public class AchievementList : ReorderableArray<Achievement> {}
}
