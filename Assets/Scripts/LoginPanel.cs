using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.SetActive(true);
        GameObject.Find("leftHand").SetActive(false);
        GameObject.Find("rightHand").SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickLoginButton()
    {
        //医生验证函数
        //病人验证函数
        if (GameObject.Find("InputID").GetComponent<InputField>().text == "1111"&& GameObject.Find("InputPassword").GetComponent<InputField>().text == "1111")
        {
            GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("DoctorStartPanel");
        }
        else if(GameObject.Find("InputID").GetComponent<InputField>().text == "2222" && GameObject.Find("InputPassword").GetComponent<InputField>().text == "2222")
        {
            GameObject.Find("Canvas").GetComponent<MainMenuManager>().OpenPanelByName("PatientStartPanel");
        }
    }
}
