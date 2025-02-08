using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Malee.List;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Choices", order = 2)]
public class Choices : ScriptableObject {
    // if set to true will require the previous ones achievement regardless of the achievements in the prefab
    public bool enforceOrder = false;
    // reordarable list of choices
    [Reorderable] public GameObjectList choices;

    [System.Serializable]
    public class GameObjectList : ReorderableArray<GameObject> {}
}
