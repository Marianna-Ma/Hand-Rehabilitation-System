using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class Admin : MonoBehaviour {
	
	public static string host = "119.3.231.171";				//IP地址
	public static string port = "3306";				//端口号
	public static string userName = "admin";			//用户名
	public static string password = "Rehabsys@2019";			//密码
	public static string databaseName = "rehabsys";		//数据库名称

	//封装好的数据库类
	MySqlAccess mysql;


	public Admin(string _host, string _port, string _userName, string _password, string _databaseName) {
		host = _host;
		port = _port;
		userName = _userName;
		password = _password;
		databaseName = _databaseName;
	}


	/// <summary>
	/// 按下UpdatePswd按钮
	/// </summary>
	public void UpdatePassword(string first_pswd, string second_pswd) {
		string userID = "";
		if (PlayerPrefs.HasKey("userID")) {
			userID = PlayerPrefs.GetString ("userID");
		}
		Debug.Log ("userID" + userID);

		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();

		string firstPswd = first_pswd;
		string secondPswd = second_pswd;

		int flag = 0;
		Debug.Log (firstPswd + " " + secondPswd);
		if (firstPswd.Length < 6 || firstPswd.Length > 20) {
			flag = 0;
			Debug.Log ("密码长度应为6~20个字符");
		}
		else {
			string query = "";
			if (userID[0] == '1')
				query = "select adm_pswd from adm where adm_id = '" + userID + "'";
			else if (userID[0] == '2')
				query = "select dc_pswd from doc where dc_id = '" + userID + "'";
			else if (userID[0] == '3')
				query = "select pt_pswd from pat where pt_id = '" + userID + "'";
			DataSet ds = mysql.QuerySet (query);
			DataTable table = ds.Tables [0];
			string beforePswd = table.Rows[0][0].ToString();
			Debug.Log ("beforePswd" + beforePswd);
			if (beforePswd == firstPswd) {
				flag = 0;
				Debug.Log ("新密码不可与原密码相同");
			} else {
				if (firstPswd != secondPswd) {
					flag = 0;
					Debug.Log ("两次密码不一致");
				} else {
					flag = 1;
				}
			}
		}

		if (flag == 1) {
			string query = "";
			if (userID[0] == '1')
				query = "update adm set adm_pswd = '" + firstPswd + "' where adm_id = '" + userID + "'";
			else if (userID[0] == '2')
				query = "update doc set dc_pswd = '" + firstPswd + "' where dc_id = '" + userID + "'";
			else if (userID[0] == '3')
				query = "update pat set pt_pswd = '" + firstPswd + "' where pt_id = '" + userID + "'";
			DataSet ds = mysql.QuerySet (query);
			Debug.Log ("修改密码成功，新密码为：" + firstPswd);
		}
		mysql.Close ();
	}


	/// <summary>
	/// 按下AddDoctor按钮
	/// </summary>
	public void AddDoctor(string dc_id, string dc_name, string dc_sex, string dc_pro, string dc_tele) {
		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		string dcID = dc_id;
		string dcName = dc_name;
		string dcSex = dc_sex;
		string dcPro = dc_pro;
		string dcTele = dc_tele;

		int flag = 1; // 1可添加，0不可添加
		if (dcID == "" | dcName == "") {
			flag = 0;
			Debug.Log ("医生编号和姓名不能为空");
		}
		if (dcID != "") {
			string query = "select * from ppl where ppl_id = " + dcID;
			DataSet ds = mysql.QuerySet (query);
			DataTable table = ds.Tables [0];
			if (table.Rows.Count == 0) {
				flag = 0;
				Debug.Log ("医生编号不在人员表中");
			}
		}
		if (flag == 1) {
			string query = "insert into doc values ('" + dcID + "','" + dcName + "','" + dcSex + "','" + dcPro + "','" + dcTele + "','" + dcID + "',1)";
			DataSet ds = mysql.QuerySet (query);
			//			DataTable table = ds.Tables [0];
			string query1 = "update ppl set ppl_act = 1 where ppl_id = '" + dcID + "'";
			DataSet ds1 = mysql.QuerySet (query1);
			Debug.Log ("添加医生账号成功，医生编号：" + dcID);
		}
		mysql.Close ();
	}

	/// <summary>
	/// 查询所有医生，返回医生编号、姓名、性别、职称、电话
	/// </summary>
	public DataTable QueryDoctor() {
		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		string query = "select dc_id, dc_name, dc_sex, dc_pro, dc_tele from doc where dc_ex = 1";
		DataSet ds = mysql.QuerySet (query);
		DataTable table = ds.Tables [0];
		mysql.Close ();
		return table;
	}


	/// <summary>
	/// 删除医生
	/// </summary>
//	public void DeleteDoctor(string[] dc_id) {
	public void DeleteDoctor() {
		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		//		string[] dcID = dc_id;
		string[] dcID = {"200001", "200002"};
		for (int i = 0; i < dcID.Length; i++) {
			string query = "update doc set dc_ex = 0 where dc_id = '" + dcID [i] + "'";
			DataSet ds = mysql.QuerySet (query);
		}
		mysql.Close ();
	}
}
