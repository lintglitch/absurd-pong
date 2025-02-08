using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selector : MonoBehaviour {
    public Choices choices;
    public string selectionSaveName;
    public TextMeshProUGUI selectField;
    public TextMeshProUGUI descriptionField;
    public bool enforceOrder = false;

    private int selection = 0;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(StartAnimation());

        // load selection from last time
        SetStartSelection();

        // set selection to the start value
        UpdateSelection(GetSelection());
    }

    public GameObject SelectIndex(int index) {
        if(index >= choices.choices.Count) {
            Debug.LogWarning("Tried to select index " + index + " for a greater number then choices " + choices.choices.Count);
            selection = 0;
            return choices.choices[0];
        }

        selection = index;
        return choices.choices[selection];
    }

    public int GetCurrentIndex() {
        if(!IsIndexUnlocked(selection)) SelectFirst();
        return selection;
    }

    // returns total amount of unlocked choices
    public int GetTotalUnlockedChoices() {
        int count = 0;
        for(int i=0; i<choices.choices.Count; i++) {
            if(IsIndexUnlocked(i)) count++;
        }
        return count;
    }

    // returns all possible choices
    public int GetTotalChoices() {
        return choices.choices.Count;
    }


    public void SelectNext() {
        GetComponent<AudioSource>().Play();

        int newSelection = selection;
        while(true) {
            newSelection++;
            if(newSelection >= choices.choices.Count) newSelection = 0;
            if(newSelection == selection) break;
            if(IsIndexUnlocked(newSelection)) break;
        }
        SetSelection(newSelection);
    }

    public void SelectPrevious() {
        GetComponent<AudioSource>().Play();

        int newSelection = selection;
        while(true) {
            newSelection--;
            if(newSelection < 0) newSelection = choices.choices.Count - 1;
            if(newSelection == selection) break;
            if(IsIndexUnlocked(newSelection)) break;
        }
        SetSelection(newSelection);
    }

    public void SelectFirst() {
        SetSelection(0);
    }

    public GameObject GetSelection() {
        return choices.choices[selection];
    }

    protected virtual void UpdateSelection(GameObject prefab) {
        Player player = prefab.GetComponent<Player>();
        selectField.text = player.name;
        descriptionField.text = player.description;
    }

    protected virtual void SetStartSelection() {
        SelectIndex(GetSavedSelection());
    }

    protected int GetSavedSelection() {
        return PlayerPrefs.GetInt("selector:" + selectionSaveName, 0);
    }

    protected void SetSelection(int newSelection) {
        selection = newSelection;
        PlayerPrefs.SetInt("selector:" + selectionSaveName, selection);
        UpdateSelection(choices.choices[selection]);
    }

    protected bool IsIndexUnlocked(int index) {
        Player player = choices.choices[index].GetComponent<Player>();

        // if no unlocks manager present just unlock everything
        if(UnlocksManager.instance == null) return true;

        // need to do stricter tests if enforce order is active
        if(enforceOrder && index > 0) {
            // if we don't have the achievement for the preivous one that don't continue
            Player prev = choices.choices[index-1].GetComponent<Player>();
            if(!UnlocksManager.instance.IsUnlocked(prev.id)) return false;
        }

        // see if achievement is explicitely required
        if(player.achievementRequired == "") return true;

        // otherwise just check achievement
        return UnlocksManager.instance.IsUnlocked(player.achievementRequired);
    }

    IEnumerator StartAnimation() {
        yield return new WaitForSeconds(0.5f);
        // if more then one choice is available play animation
        if(GetTotalUnlockedChoices() > 1) {
            Animator animator = gameObject.GetComponent<Animator>();
            if(animator) animator.Play("Arrow");
        }
        yield break;
    }
}
