using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleVersus : PersistentToggle {
    public GameObject chooseEnemy;
    public GameObject choosePlayer2;
    public Toggle coopToggle;
    public Selectable player1ChoiceButtonLeft;
    public Selectable player1ChoiceButtonRight;
    public Selectable player2ChoiceButtonLeft;
    public Selectable player2ChoiceButtonRight;
    public Selectable enemyChoiceButtonLeft;
    public Selectable enemyChoiceButtonRight;
    public Selectable startButton;

    public void Toggle() {
        bool playSound = true;

        Toggle toggle = gameObject.GetComponent<Toggle>();
        if(toggle.isOn) {
            chooseEnemy.SetActive(false);
            choosePlayer2.SetActive(true);
            RewireForVersusSelect();
            if (coopToggle.isOn) {
                coopToggle.isOn = false;
                playSound = false;
            }
            MasterController.instance.versusActive = true;
        }
        else {
            chooseEnemy.SetActive(true);
            choosePlayer2.SetActive(false);
            RewireForEnemySelect();
            MasterController.instance.versusActive = false;
        }

        PlayerManagerController.instance.UpdateJoining();
        DoToggle(playSound);
    }

    // to ensure it still remains controllable via controller this hacky solution
    private void RewireForVersusSelect() {
        Navigation n;
        n = player1ChoiceButtonLeft.navigation;
        n.selectOnDown = player2ChoiceButtonLeft;
        player1ChoiceButtonLeft.navigation = n;

        n = player1ChoiceButtonRight.navigation;
        n.selectOnDown = player2ChoiceButtonRight;
        player1ChoiceButtonRight.navigation = n;

        n = startButton.navigation;
        n.selectOnLeft = player2ChoiceButtonRight;
        startButton.navigation = n;
    }

    // and another one
    private void RewireForEnemySelect() {
        Navigation n;
        n = player1ChoiceButtonLeft.navigation;
        n.selectOnDown = enemyChoiceButtonLeft;
        player1ChoiceButtonLeft.navigation = n;

        n = player1ChoiceButtonRight.navigation;
        n.selectOnDown = enemyChoiceButtonRight;
        player1ChoiceButtonRight.navigation = n;

        n = startButton.navigation;
        n.selectOnLeft = enemyChoiceButtonRight;
        startButton.navigation = n;
    }

}
