﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowHue : MonoBehaviour
{
    public float Speed = 1;
    private Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
    }

    void Update() {
        rend.material.SetColor("_Color", HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * Speed, 1), 1, 1)));
    }
}
