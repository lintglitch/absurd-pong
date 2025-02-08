using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonsMainMenu : MonoBehaviour {
    public bool centerButtonControl = false;
    public string transitionName;
    public string previousState = "main";
    public Selectable selectNext;

    private Animator animator;

    void Start() {
        animator = Camera.main.GetComponent<Animator>();
    }

    public void TowardsButton() {
        TransitionButtonPress(true);
        MasterController.instance.SetCurrentMenu(transitionName);

        if (centerButtonControl) DisableCenterButtons();
    }

    public void BackButton() {
        TransitionButtonPress(false);
        MasterController.instance.SetCurrentMenu(previousState);

        if (centerButtonControl) EnableCenterButtons();
    }

    public void DeactivateState(string state) {
        animator.SetBool(state, false);
    }

    public void EnableCenterButtons() {
        GameObject[] buttonObjs = GameObject.FindGameObjectsWithTag("CenterButtons");
        foreach(GameObject buttonObj in buttonObjs) {
            buttonObj.GetComponent<Button>().interactable = true;
        }
    }

    public void DisableCenterButtons() {
        GameObject[] buttonObjs = GameObject.FindGameObjectsWithTag("CenterButtons");
        foreach (GameObject buttonObj in buttonObjs) {
            buttonObj.GetComponent<Button>().interactable = false;
        }
    }

    public void QuitButton() {
        GameObject obj = GameObject.FindWithTag("MenuCanvas");
        if (obj != null) {
            obj.GetComponent<OverlayMenu>().ShowEscapeMenu(gameObject.GetComponent<Button>());
        }
        else {
            Application.Quit();
        }
    }

    public void ResetAchievements() {  
        UnlocksManager.instance.ResetAchievements();
        PressButton();
    }

    public void UnlockAllAchievements() {
        UnlocksManager.instance.UnlockEverything();
        PressButton();
    }

    private void TransitionButtonPress(bool state) {
        animator.SetBool(transitionName, state);

        if(selectNext != null) {
            selectNext.Select();
        }

        PressButton();
    }

    private void PressButton() {
        GetComponent<AudioSource>().Play();
    }
}
