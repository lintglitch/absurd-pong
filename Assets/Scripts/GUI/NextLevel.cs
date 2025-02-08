using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {
    void Start() {
        // if we just had a match go back to the updated selection menu
        if(MasterController.instance.backToSelection) {
            MasterController.instance.backToSelection = false;
            gameObject.GetComponent<Animator>().SetBool("Selection", true);
        }
    }

    public void OnFadeComplete(string level) {
        SceneManager.LoadScene(level);
    }
}
