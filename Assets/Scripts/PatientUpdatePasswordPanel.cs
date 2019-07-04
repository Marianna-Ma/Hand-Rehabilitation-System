using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientUpdatePasswordPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickCancelButton()
    {
        //Debug.Log(GameObject.Find("canvas"))
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientStartPanel");
    }
}
