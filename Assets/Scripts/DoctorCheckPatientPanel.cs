using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorCheckPatientPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickAddPatientButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorAddPatientPanel");
    }

    public void ClickDeletePatientButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorDeletePatientPanel");
    }

    public void ClickBackButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorDeletePatientPanel");
    }
}
