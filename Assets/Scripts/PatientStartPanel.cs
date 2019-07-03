using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientStartPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //跳转去个人信息修改界面
    public void ClickPatientChangeInfoButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientChangeInfoPanel");
    }

    //跳转至训练界面
    public void ClickPatientStartButton()
    {
        GameObject.Find("leftHand").SetActive(true);
        GameObject.Find("rightHand").SetActive(true);
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientTrainPanel");

    }

    //跳转去查看记录界面
    public void ClickPatientCheckButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientHistoryRecordPanel");
    }

    //跳转至登录界面
    public void ClickQuitButton()
    {
        //注销player
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPreviousPanel();
    }
}
