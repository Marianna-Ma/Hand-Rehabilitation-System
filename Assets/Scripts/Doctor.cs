using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class Doctor : MonoBehaviour {
	
	public static string host;				//IP地址
	public static string port;				//端口号
	public static string userName;			//用户名
	public static string password;			//密码
	public static string databaseName;		//数据库名称

	//封装好的数据库类
	MySqlAccess mysql;


	public Doctor(string _host, string _port, string _userName, string _password, string _databaseName) {
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
	/// 按下Change Info按钮
	/// </summary>
	public void ChangeInfo(string dc_name, string dc_sex, string dc_pro, string dc_tele) {
		string dcID = "";
		if (PlayerPrefs.HasKey("userID")) {
			dcID = PlayerPrefs.GetString ("userID");
		}
		Debug.Log ("userID" + dcID);

		string dcName = dc_name;
		string dcSex = dc_sex;
		string dcPro = dc_pro;
		string dcTele = dc_tele;

		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		string query = "update doc set dc_name = '" + dcName + "', dc_sex = '" + dcSex + "', dc_pro = '" + dcPro + "', dc_tele = '" + dcTele + "' where dc_id = '" + dcID + "'";
		DataSet ds = mysql.QuerySet (query);
		DataTable table = ds.Tables [0];

		mysql.Close ();
	}



	/// <summary>
	/// 按下Add Patient按钮
	/// </summary>
	public void AddPatient(string pt_id, string pt_name, string pt_sex, string pt_tele) {
		string dcID = "";
		if (PlayerPrefs.HasKey("userID")) {
			dcID = PlayerPrefs.GetString ("userID");
		}
		Debug.Log ("userID" + dcID);

		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		string ptID = pt_id;
		string ptName = pt_name;
		string ptSex = pt_sex;
		string ptTele = pt_tele;
		int flag = 1; // 1可添加，0不可添加
		if (ptID == "" | ptName == "") {
			flag = 0;
			Debug.Log ("患者编号和姓名不能为空");
		}
		if (ptID != "") {
			string query = "select * from ppl where ppl_id = " + ptID;
			DataSet ds = mysql.QuerySet (query);
			DataTable table = ds.Tables [0];
			if (table.Rows.Count == 0) {
				flag = 0;
				Debug.Log ("患者编号不在人员表中");
			}
		}
		if (flag == 1) {
			string query = "insert into pat values ('" + ptID + "','" + ptName + "','" + ptSex + "','" + ptTele + "','" + ptID + "','" + dcID + "',1)";
			DataSet ds = mysql.QuerySet (query);
			string query1 = "update ppl set ppl_act = 1 where ppl_id = '" + ptID + "'";
			DataSet ds1 = mysql.QuerySet (query1);
			Debug.Log ("添加患者账号成功，患者编号：" + ptID);
		}
		mysql.Close ();
	}

	/// <summary>
	/// 查询某个医生的所有患者，返回患者编号、姓名、性别、电话
	/// </summary>
	public DataTable QueryPatient() {
		string dcID = "";
		if (PlayerPrefs.HasKey("userID")) {
			dcID = PlayerPrefs.GetString ("userID");
		}
		Debug.Log ("userID" + dcID);

		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
		string query = "select pt_id, pt_name, pt_sex, pt_tele from pat where pt_dcID = '" + dcID + "' and pt_ex = 1";
		DataSet ds = mysql.QuerySet (query);
		DataTable table = ds.Tables [0];
		mysql.Close ();
		return table;
	}


	/// <summary>
	/// 删除患者
	/// </summary>
//	public void DeletePatient(string[] pt_id) {
	public void DeletePatient() {
		string dcID = "";
		if (PlayerPrefs.HasKey ("userID")) {
			dcID = PlayerPrefs.GetString ("userID");
		}
		Debug.Log ("userID" + dcID);

		mysql = new MySqlAccess(host, port, userName, password, databaseName);
		mysql.OpenSql ();
//		string[] ptID = pt_id;
		string[] ptID = {"300002", "300003"};
		for (int i = 0; i < ptID.Length; i++) {
			string query = "update pat set pt_ex = 0 where pt_dcID = '" + dcID + "' and pt_id = '" + ptID [i] + "'";
			DataSet ds = mysql.QuerySet (query);
		}
		mysql.Close ();
	}
}
