using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private int esc;
	// Use this for initialization
	void Start () {
        esc = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) esc++;
        if (2 == esc) Application.Quit();
	}
}
