using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class AdminUI : MonoBehaviour, IPointerClickHandler {
	// 修改密码 面板的输入框
	public InputField adm_firstPswdField;
	public InputField adm_secondPswdField;

	// 添加医生 面板的输入框
	public InputField dcIDField;
	public InputField dcNameField;
	public InputField dcSexField;
	public InputField dcProField;
	public InputField dcTeleField;

	public string host;				//IP地址
	public string port;				//端口号
	public string userName;			//用户名
	public string password;			//密码
	public string databaseName;		//数据库名称

	public Admin test;

	// Use this for initialization
	void Start () {
		test = new Admin(host, port, userName, password, databaseName);
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.pointerPress.name == "updatePswdButton") 	//如果当前按下的按钮是修改密码按钮
			test.UpdatePassword(adm_firstPswdField.text.ToString(), adm_secondPswdField.text.ToString());
		if (eventData.pointerPress.name == "addDoctorButton")	//如果当前按下的按钮是添加医生按钮
			test.AddDoctor(dcIDField.text.ToString(), dcNameField.text.ToString(), dcSexField.text.ToString(), dcProField.text.ToString(), dcTeleField.text.ToString());
		if (eventData.pointerPress.name == "deleteDoctorButton")	//如果当前按下的按钮是删除患者按钮
			test.DeleteDoctor();
	}

}
