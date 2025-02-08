using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectorOpponent : Selector {
    public int numberOfQuestionMarks = 100;
    public Color normalColor;
    public Color bossColor;

    protected override void UpdateSelection(GameObject prefab) {
        Player player = prefab.GetComponent<Player>();
        selectField.text = player.name;
        descriptionField.text = GetBoxString(player);

        // change color based on the boss status
        if(player.bossIntroduction) {
            selectField.color = bossColor;
        }
        else {
            selectField.color = normalColor;
        }
    }

    protected override void SetStartSelection() {
        int index = GetSavedSelection();

        // check master controller
        if(MasterController.instance.selectNextEnemy) {
            SelectNext();
            MasterController.instance.selectNextEnemy = false;
        }

        SelectIndex(index);
    }

    private string GetBoxString(Player player) {
        if (UnlocksManager.instance.GetNumberMatches(player.id) == 0) {
            return new string('?', numberOfQuestionMarks);
        }

        return player.description + GetStatisticsString(player.id, player);
    }

    private string GetStatisticsString(string id, Player opponent) {
        // if we haven't encountered that opponent even once
        int plays = UnlocksManager.instance.GetNumberMatches(id);
        int wins = UnlocksManager.instance.GetNumberWins(id);

        string statistics = string.Format("\n\nPower level: {0}\nMatches: {1}, Wins: {2}\n", opponent.powerLevelDescription, plays, wins);

        bool defeatedUsingHexHex = UnlocksManager.instance.GetNumberWinsWithPaddle(id, "HexHex") > 0;
        if(defeatedUsingHexHex) {
            statistics += "HexHex";
        }
        return statistics;
    }
}
