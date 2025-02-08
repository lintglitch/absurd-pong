using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// controls across all scenes
public class MasterController : MonoBehaviour {
    public static MasterController instance = null;
    public GameObject player1Choice = null;
    public GameObject player2Choice = null;
    public GameObject coopChoice = null;

    public bool versusActive = false;
    public bool coopActive = false;

    public bool selectNextEnemy = false;
    public bool backToSelection = false;

    private string currentlyInMenu = "main";

    // singleton
    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            print("Initialized!");
        }

        // destroy yourself if already existing
        else {
            Destroy(gameObject);
        }
    }

    public bool IsMultiplayer() {
        return versusActive || coopActive;
    }

    public void SetCurrentMenu(string name) {
        currentlyInMenu = name;

        // perform enabled update if currently in 
        if(name == "Selection" && PlayerManagerController.instance != null) {
            PlayerManagerController.instance.UpdateJoining();
        }
    }

    public string GetCurrentMenu() {
        return currentlyInMenu;
    }
}
