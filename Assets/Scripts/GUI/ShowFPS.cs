﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowFPS : MonoBehaviour {
    public TextMeshProUGUI counterField;
    private float deltaTime;

    void Update() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        counterField.text = Mathf.Ceil(fps).ToString();
    }
}
