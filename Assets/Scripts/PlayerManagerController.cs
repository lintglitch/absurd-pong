using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManagerController : MonoBehaviour {
    public static PlayerManagerController instance;
    public float pitchChange;

    private List<PlayerInput> inputList;
    private AudioSource audioSource;

    // singleton
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            print("Initialized!");
        }

        // destroy yourself if already existing
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        inputList = new List<PlayerInput>();
        audioSource = gameObject.GetComponent<AudioSource>();
        // apply first pitch change early so we get the 1.0 pitch for the first join
        audioSource.pitch -= pitchChange;
    }

    public void OnPlayerJoined(PlayerInput playerInput) {
        print("Joined " + playerInput.devices[0].displayName);
        inputList.Add(playerInput);
        audioSource.pitch += pitchChange;
        audioSource.Play();
    }

    public void UpdateJoining() {
        MasterController master = MasterController.instance;
        PlayerInputManager manager = PlayerInputManager.instance;

        if(master == null || master.GetCurrentMenu() != "Selection" || (!master.coopActive && !master.versusActive)) {
            manager.DisableJoining();
        }
        else {
            manager.EnableJoining();
        }
    }

    public List<PlayerInput> GetPlayerInputs() {
        return inputList;
    }

    public int GetNumberOfPlayers() {
        return inputList.Count;
    }

    public string GeneratePlayerInputsString() {
        string s = "";
        foreach(PlayerInput input in inputList) {
            s += "Player " + (input.playerIndex+1) + ": " + input.devices[0].displayName + "\n";
        }
        return s;
    }

    // removes the paddles that players currently control
    public void ClearPlayerControlledPaddles() {
        foreach (PlayerInput input in inputList) {
            PlayerController controller = input.GetComponent<PlayerController>();
            controller.RemoveChildren();
        }
    }

    // remove all saved players within the controller
    public void Clear() {
        inputList.Clear();
    }
}
