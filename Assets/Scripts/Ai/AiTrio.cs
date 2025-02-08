using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTrio : AiSimple {
    public SpriteRenderer follower1;
    public SpriteRenderer follower2;

    public override void Hide() {
        base.Hide();
        follower1.enabled = false;
        follower2.enabled = false;
    }

    public override void Show() {
        base.Show();
        follower1.enabled = true;
        follower2.enabled = true;
    }
}
