using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OverlayMenu : MonoBehaviour {
    public GameObject escapeMenu;
    public Selectable escapeMenuDefaultSelected;
    public AudioMixer masterMixer;
    public string musicChangeAttribute;
    public float musicChangeValue;
    public AudioClip back;

    private bool escapeMenuOpen = false;
    private float previousMusicChangeValue;
    private Selectable previousSelected;

    public void ToggleEscapeMenu() {
        if (GameControl.instance == null) return;

        if(escapeMenuOpen) {
            CloseEscapeMenu();
        }
        else {
            ShowEscapeMenu();
        }
    }

    public void ShowEscapeMenu(Selectable previousSelected = null) {
        escapeMenu.SetActive(true);
        this.previousSelected = previousSelected;
        escapeMenuOpen = true;
        escapeMenuDefaultSelected.Select();
        masterMixer.GetFloat(musicChangeAttribute, out previousMusicChangeValue);
        masterMixer.SetFloat(musicChangeAttribute, musicChangeValue);

        // pause the game if it is currently running
        if (GameControl.instance != null) {
            GameControl.instance.PauseGame();
        }
    }

    public void Quit() {
        masterMixer.SetFloat(musicChangeAttribute, previousMusicChangeValue);

        // if the game controller is running use its end function instead of closing the whole application
        if (GameControl.instance != null) {
            GameControl.instance.EndGame();
        }
        else {
            Application.Quit();
        }
    }

    public void CloseEscapeMenu() {
        escapeMenu.SetActive(false);
        escapeMenuOpen = false;
        masterMixer.SetFloat(musicChangeAttribute, previousMusicChangeValue);
        gameObject.GetComponent<AudioSource>().PlayOneShot(back);

        if(previousSelected != null) {
            previousSelected.Select();
        }

        // unpause the game if it is currently running
        if (GameControl.instance != null) {
            GameControl.instance.UnpauseGame();
        }
    }
}