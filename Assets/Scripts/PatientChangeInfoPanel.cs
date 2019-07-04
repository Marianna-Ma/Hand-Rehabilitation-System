using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientChangeInfoPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickChangeInfoButton()
    {
        //修改个人信息
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientStartPanel");
    }

    public void ClickCancelButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientStartPanel");
    }

    public void ClickUpdatePasswordButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientUpdatePasswordPanel");
    }
}
