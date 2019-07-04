using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorChangeInfoPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickChangeInfoPanel()
    {
        //Debug.Log(GameObject.Find("canvas"))
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorStartPanel");
    }

    public void ClickCancelChangePanel()
    {
        //Debug.Log(GameObject.Find("canvas"))
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorStartPanel");
    }
}
