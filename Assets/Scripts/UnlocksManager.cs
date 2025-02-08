using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlocksManager : MonoBehaviour {
    public AchievementData data;
    public int numberOfDefeatedEnemiesForFullUnlock;
    public Choices enemies;
    [HideInInspector] public static UnlocksManager instance = null;

    // tracks unlocks based on id
    private Dictionary<string, bool> unlockCompletion = null;

    // check if achievement unlocked
    public bool IsUnlocked(string id) {
        if(unlockCompletion == null) return true;
        if(!unlockCompletion.ContainsKey(id)) {
            Debug.LogError("Tried to access unknown achievement: " + id);
            return false;
        }
        return unlockCompletion[id];
    }

    // unlock an achievement, returns true if successfully unlocked new achievement
    public bool Unlock(string id, bool showAnimation=true) {
        if(!unlockCompletion.ContainsKey(id)) {
            Debug.LogError("Tried to unlock nonexisting achievement: " + id);
            return false;
        }

        // check if achievement is already unlocked
        if(unlockCompletion[id]) {
            return false;
        }

        // try to get the achievement
        Achievement a = data.GetAchievement(id);
        if(a == null) {
            Debug.LogError("Error getting the achievement: " + id);
            return false;
        }

        unlockCompletion[id] = true;
        PlayerPrefs.SetInt("unlock:" + id, 1);
        PlayerPrefs.Save();

        // refresh unlock menu
        RefreshUnlockMenu();

        // display
        if(showAnimation) {
            ShowAchievement(a);
        }

        return true;
    }

    public void UnlockEverything() {
        foreach (Achievement a in data.achievements) {
            string id = a.id;
            Unlock(id, false);
        }
    }

    public void RegisterMatch(string player1, string player2, bool player1Won, bool humanMatch, bool coopMatch) {
        // for local coop matches there is not much todo except unlocking the achievement
        if(humanMatch == true) {
            Unlock("Multiplayer");
            Debug.Log("Unlocking multiplayer achievement");
            return;
        }

        string id = player2;
        IncreaseNumberMatches(id);
        Debug.Log("Registering Match for " + id + " and " + player1);

        if(player1Won) {
            IncreaseNumberWins(id, player1);
            Achievement a = TryDefeatUnlock(id, player1);
            if(a != null) {
                Debug.Log("UNLOCKED " + a.id);

                // if the new achievement just unlocked the next enemy then mark that
                if(a.unlocksNext) {
                    MasterController.instance.selectNextEnemy = true;
                    Debug.Log("SETTING TO NEXT");
                }
            }

            TryDefeatAllUnlock(id, player1);
        }
    }

    public int GetNumberMatches(string id) {
        int plays = PlayerPrefs.GetInt(id + ":plays", 0);
        return plays;
    }

    public void IncreaseNumberMatches(string id) {
        int plays = GetNumberMatches(id);
        PlayerPrefs.SetInt(id + ":plays", plays + 1);
    }

    public int GetNumberWins(string id) {
        int wins = PlayerPrefs.GetInt(id + ":wins", 0);
        return wins;
    }

    public int GetNumberWinsWithPaddle(string id, string paddle) {
        int wins = PlayerPrefs.GetInt(id + ":vs:" + paddle, 0);
        return wins;
    }

    public void IncreaseNumberWins(string id, string paddle) {
        int wins = GetNumberWins(id);
        PlayerPrefs.SetInt(id + ":wins", wins + 1);
        PlayerPrefs.SetInt(id + ":vs:" + paddle, 1);
    }

    // returns true if defeating this enemy could unlock something with the given pad
    public bool DoesDefeatUnlock(string defeated, string paddleUsed = "") {
        var entries = GetDefeatUnlockIDs(defeated, paddleUsed);
        if(entries != null) return true;
        return false;
    }

    // tries to get an achievement that is not yet unlocked, can possibly unlocked several achievements at once
    public Achievement TryDefeatUnlock(string defeated, string paddleUsed = "") {
        var entries = GetDefeatUnlockIDs(defeated, paddleUsed);
        if(entries == null) return null;

        Achievement returnAchievement = null;
        foreach(var entry in entries) {
            // save the first newly unlocked achievement for return
            if (Unlock(entry) && returnAchievement == null) {
                returnAchievement = data.GetAchievement(entry);
            }
        }

        return returnAchievement;
    }

    public Achievement TryDefeatAllUnlock(string defeated, string paddleUsed = "") {
        Achievement bufferedAchievement = null;
        foreach (Achievement a in data.achievements) {
            if(a.defeatAllWithPaddle) {
                if(a.paddleNeeded == null) {
                    Debug.LogError("Achievement " + a.id + " requires defeat all with paddle but has no paddle assigned");
                    continue;
                }

                if (IsUnlocked(a.id)) continue;

                int count = 0;
                foreach(var enemyChoice in enemies.choices) {
                    string id = enemyChoice.GetComponent<Player>().id;
                    if(GetNumberWinsWithPaddle(id, paddleUsed) > 0) {
                        count += 1;
                    }
                }

                if(count >= numberOfDefeatedEnemiesForFullUnlock) {
                    Unlock(a.id);
                    bufferedAchievement = a;
                }
            }
        }
        return bufferedAchievement;
    }

    // deletes all achievements
    public void ResetAchievements() {
        foreach(Achievement a in data.achievements) {
            string id = a.id;
            PlayerPrefs.SetInt("unlock:" + id, 0);
            unlockCompletion[id] = false;
        }

        RefreshUnlockMenu();
    }

    public int GetAchievedUnlocks() {
        int counter = 0;
        foreach (Achievement a in data.achievements) {
            if (unlockCompletion[a.id]) counter++;
        }
        return counter;
    }

    public int GetNumberPossibleUnlocks() {
        return unlockCompletion.Count;
    }

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // destroy yourself if already existing
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        LoadCompletions();
        RefreshUnlockMenu();
    }

    private void LoadCompletions() {
        // create unlock completion array
        unlockCompletion = new Dictionary<string, bool>();

        // load unlock completion status from disk
        foreach(Achievement a in data.achievements) {
            string id = a.id;

            // check that the id has not already been used
            if(unlockCompletion.ContainsKey(id)) {
                Debug.LogError("Unlock manager tried to load achievement key twice: '" + id + "', " + a.name);
            }

            unlockCompletion.Add(id, PlayerPrefs.GetInt("unlock:" + id, 0) == 1);
        }
    }

    private void RefreshUnlockMenu() {
        GameObject obj = GameObject.FindWithTag("AchievementMenu");
        if(obj != null) {
            UnlocksPanelAdder panelManager = obj.GetComponent<UnlocksPanelAdder>();
            panelManager.Refresh();
        }
    }

    private void ShowAchievement(Achievement achievement) {
        GameObject obj = GameObject.FindWithTag("AchievementShower");
        if(obj != null) {
            AchievementShow shower = obj.GetComponent<AchievementShow>();
            string description;
            if(achievement.finishedDescription != "") description = achievement.finishedDescription;
            else description = achievement.description;
            shower.ShowAchievement(achievement.name, description);
        }

    }

    // gets all matching defeat unlocks
    private List<string> GetDefeatUnlockIDs(string defeated, string paddleUsed) {
        var unlockIDs = new List<string>();

        foreach(Achievement a in data.achievements) {
            string id = a.id;

            if(a.DoesDefeatUnlock(defeated, paddleUsed)) {
                unlockIDs.Add(a.id);
            }
        }

        if (unlockIDs.Count == 0) return null;

        return unlockIDs;
    }
}
