using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonStart : MonoBehaviour {
    public Selector player1;
    public Selector enemy;
    public Selector player2;
    public Animator camera;
    public Toggle versusToggle;
    public Toggle coopToggle;
    public TextMeshProUGUI notEnoughPlayersText;
    public float showNotEnoughPlayersTime;
    public AudioClip success;
    public AudioClip denied;

    public void Click() {
        // check if we have enough players for multiplayer
        if(MasterController.instance.IsMultiplayer() && PlayerManagerController.instance.GetNumberOfPlayers() < 2) {
            GetComponent<AudioSource>().PlayOneShot(denied, 1.0f);
            StartCoroutine(ShowMissingPlayerForShortWhile());
        }
        else {
            StartGame();
        }
    }

    private void StartGame() {
        // set all given values in master
        MasterController controller = MasterController.instance;
        controller.player1Choice = player1.GetSelection();

        // fight between players
        if (versusToggle.isOn) {
            controller.player2Choice = player2.GetSelection();
        }
        else {
            // otherwise we fight against ai
            controller.player2Choice = enemy.GetSelection();
        }

        PlayerPrefs.Save();
        MasterController.instance.SetCurrentMenu("main");
        camera.SetBool("FadeLevel", true);
        GetComponent<AudioSource>().PlayOneShot(success, 0.4f);
    }

    IEnumerator ShowMissingPlayerForShortWhile() {
        notEnoughPlayersText.gameObject.SetActive(true);
        yield return new WaitForSeconds(showNotEnoughPlayersTime);
        notEnoughPlayersText.gameObject.SetActive(false);
    }
}
