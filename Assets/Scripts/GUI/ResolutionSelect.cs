using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSelect : MonoBehaviour {
    // a list of accetable horizontal resolutions
    public int[] acceptableResolutionsHorizontal;
    public Toggle fullscreenToggle;

    private bool active = false;
    private TMP_Dropdown dropdown;
    private const string RESOLUTION_KEY = "resolution";
    private List<Resolution> resolutionOptions;

    public void SetSelectedResolution() {
        if(!active) return;
        print("Was set");
        Resolution resolution = resolutionOptions[dropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
    }

    // Start is called before the first frame update
    void Start() {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();

        int selected = AddResolutions();
        dropdown.value = selected;
        active = true;
        //dropdown;
    }

    // adds all resolution and returns the index of the currently selected one
    private int AddResolutions() {
        Resolution[] Allresolutions = Screen.resolutions;
        Resolution current = Screen.currentResolution;
        int ourResolution = 0;

        List<string> options = new List<string>();
        resolutionOptions = new List<Resolution>();
        int i = 0;
        foreach(Resolution resolution in Allresolutions) {
            print(resolution);
            // check if its the currently active resolution
            if(resolution.width == current.width && resolution.height == current.height) {
                ourResolution = i;
            }
            else if(!ResolutionOK(resolution)) continue;

            string name = resolution.ToString();
            options.Add(name);
            resolutionOptions.Add(resolution);
            i++;
        }
        dropdown.AddOptions(options);
        return ourResolution;
    }

    private bool ResolutionOK(Resolution resolution) {
        bool acceptedWidth = false;
        foreach(int acceptable in acceptableResolutionsHorizontal) {
            if(acceptable == resolution.width) {
                acceptedWidth = true;
                break;
            }
        }

        if(!acceptedWidth) return false;

        float width = resolution.width;
        float height = resolution.height;
        if( width/height >= 1.6f) return true;
        return false;
    }
}
