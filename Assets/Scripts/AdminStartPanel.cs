using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminStartPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //跳转到医生管理界面
    public void ClickAdminDoctorButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("");
    }

    //跳转到训练计划管理界面
    public void ClickAdminTrainPlanButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("");
    }

    //跳转到查看病人信息界面
    public void ClickAdminPatientButton()
    {
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("");
    }

    //退回登录界面
    public void ClickQuitButton()
    {
        //此处是否要注销player？
        GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPreviousPanel();
    }

}
