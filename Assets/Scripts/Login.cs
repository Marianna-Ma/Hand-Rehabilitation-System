using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class Login : MonoBehaviour, IPointerClickHandler {
	// loginPanel中的输入框
	public InputField userNameField;
	public InputField passwordNameField;

	public string host;				//IP地址
	public string port;				//端口号
	public string userName;			//用户名
	public string password;			//密码
	public string databaseName;		//数据库名称

	//封装好的数据库类
	MySqlAccess mysql;

	private void Start() {
//		loginMessage = GameObject.FindGameObjectWithTag("LoginMessage").GetComponent<Text>();
		InitialAdmin ();
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.pointerPress.name == "LoginButton")       //如果当前按下的按钮是注册按钮 
			LoginButton();
	}

	/// <summary>
	/// 初始化管理员表（将人员表中所有的管理员添加到管理员表中）
	/// </summary>
	public void InitialAdmin() {
		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		string query = "select ppl_id from ppl where ppl_role = '管理员'";
		DataSet ds1 = mysql.QuerySet (query);
		if (ds1 != null) {
			DataTable table = ds1.Tables [0];
			for (int i = 0; i < table.Rows.Count; i++) {
				Debug.Log (table.Rows [i] [0]);
				// 把激活改成1
				string actquery = "update ppl set ppl_act = 1 where ppl_id = " + table.Rows[i][0];
				DataSet ds2 = mysql.QuerySet (actquery);
				// 往管理员表里加
				string dupliquery = "select * from adm where adm_id = " + table.Rows[i][0];
				DataSet ds3 = mysql.QuerySet (dupliquery);
				if (ds3 == null) {
					string addquery = "insert into adm values ('" + table.Rows[i][0] + "', '" + table.Rows[i][0] + "')";
					DataSet ds4 = mysql.QuerySet (addquery);
				}
			}
			Debug.Log ("管理员表初始化完成！");
		} else {
			Debug.Log ("人员表中没有管理员身份的用户！");
		}
		mysql.Close ();
	}


	/// <summary>
	/// 按下Login按钮
	/// </summary>
	public void LoginButton() {
		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql();
		string id = userNameField.text;
		string pswd = passwordNameField.text;
		if (id [0] == '1') {
			//管理员
			string query = "select * from adm where adm_id = " + id + " and adm_pswd = " + pswd;
			DataSet ds = mysql.QuerySet (query);
			DataTable table = ds.Tables [0];
			if (table.Rows.Count != 0) {
				Debug.Log ("管理员" + id + "，登录成功！");
				PlayerPrefs.SetString ("userID", userNameField.text);
			} else {
				Debug.Log ("管理员，登录失败，没有此账号或密码错误！");
			}

		} else if (id [0] == '2') {
			//医生
			string query = "select * from doc where dc_id = " + id + " and dc_pswd = " + pswd;
			DataSet ds = mysql.QuerySet (query);
			DataTable table = ds.Tables [0];
			if (table.Rows.Count != 0) {
				Debug.Log ("医生" + id + "，登录成功！");
				PlayerPrefs.SetString ("userID", userNameField.text);
			} else {
				Debug.Log ("医生，登录失败，没有此账号或密码错误！");
			}
		} else if (id [0] == '3') {
			//患者
			string query = "select * from pat where pt_id = " + id + " and pt_pswd = " + pswd;
			DataSet ds = mysql.QuerySet (query);
			DataTable table = ds.Tables [0];
			if (table.Rows.Count != 0) {
				Debug.Log ("患者" + id + "，登录成功");
				PlayerPrefs.SetString ("userID", userNameField.text);
			} else {
				Debug.Log ("患者，登录失败，没有此账号或密码错误！");
			}
		} else {
			Debug.Log ("用户账号类型错误！");
		}
		mysql.Close();
	}

}
