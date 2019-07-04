using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorAddPatientPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickAddPatientPanel()
    {
        //Debug.Log(GameObject.Find("canvas"))
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorCheckPatientPanel");
    }
}
