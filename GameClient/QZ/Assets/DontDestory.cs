﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour {

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
