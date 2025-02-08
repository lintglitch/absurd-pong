using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnlocksPanelAdder : MonoBehaviour {
    public GameObject unlockPrefab;
    public Color notUnlockedColor;
    public Color unlockedFeatureColor;
    public int itemsPerPage;
    public int currentPage = 0;
    public int maxPages = 1;
    public TextMeshProUGUI pageNumberDisplay;

    public void Draw() {
        // add all achievements to the panel
        GameObject parent = transform.gameObject;

        // how many unlocks to skip because they are not on the page
        int skips = 0;
        if(currentPage > 0) {
            skips = currentPage * itemsPerPage;
        }

        Achievement[] unlocks = UnlocksManager.instance.data.achievements.ToArray();
        if(unlocks == null) return;

        int counter = 0;
        int unlocksDrawn = 0;
        foreach(Achievement unlock in unlocks) {
            counter++;
            // skip unlocks that are not on this page
            if(counter <= skips) continue;

            GameObject panel = Instantiate(unlockPrefab, Vector3.zero, Quaternion.identity);

            // set texts
            TextMeshProUGUI titleObj = panel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            titleObj.text = unlock.name;
            TextMeshProUGUI descriptionObj = panel.transform.Find("Description").GetComponent<TextMeshProUGUI>();

            // make it transparent if not achieved yet
            if(!UnlocksManager.instance.IsUnlocked(unlock.id)) {
                titleObj.color = notUnlockedColor;
                descriptionObj.color = notUnlockedColor;
                descriptionObj.text = unlock.description;
            }
            // if completed check if it has a completion description
            else {
                if(unlock.HasCompletedDescription()) {
                    descriptionObj.color = unlockedFeatureColor;
                    descriptionObj.text = unlock.finishedDescription;
                }
                else descriptionObj.text = unlock.description;
            }

            // set panel as child
            panel.transform.SetParent(parent.transform);

            // stop drawing if the page is full
            unlocksDrawn++;
            if(unlocksDrawn >= itemsPerPage) break;
        }

        // set the page text
        pageNumberDisplay.text = string.Format("{0}/{1}", currentPage+1, maxPages);
    }

    public void NextPage() {
        GetComponent<AudioSource>().Play();
        currentPage++;
        if(currentPage >= maxPages) currentPage = 0;
        Refresh();
    }

    public void PreviousPage() {
        GetComponent<AudioSource>().Play();
        currentPage--;
        if(currentPage < 0) currentPage = maxPages - 1;
        Refresh();
    }

    public void Refresh() {
        // destroy all unlock panels
        foreach (Transform child in gameObject.transform) {
            GameObject.Destroy(child.gameObject);
        }

        // recreate
        Draw();
    }

    void Start() {
        Draw();
    }
}
