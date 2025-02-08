using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HumanPlayerTwins : HumanPlayer {
    public SpriteRenderer follower1;
    public SpriteRenderer follower2;

    public override void Hide() {
        follower1.enabled = false;
        follower2.enabled = false;
    }

    public override void Show() {
        follower1.enabled = true;
        follower2.enabled = true;
    }
}
