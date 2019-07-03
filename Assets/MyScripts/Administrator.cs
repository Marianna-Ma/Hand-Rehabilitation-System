using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

public class Administrator : MonoBehaviour {
    //IP地址
    public static string host = "119.3.231.171";
    //端口号
    public static string port = "3306";
    //用户名
    public static string userName = "admin";
    //密码
    public static string password = "Rehabsys@2019";
    //数据库名称
    public static string databaseName = "rehabsys";
    MySqlAccess mysql;

    // Use this for initialization
    void Start () {
        mysql = new MySqlAccess(host, port, userName, password, databaseName);

        //string[,] pat_infos = getPatientInfos();
        //showDoubleArrays(pat_infos);

        //string[,] pat_plans = getPatientPlans("300001");
        //showDoubleArrays(pat_plans);

        //string[,] pat_test = getPatientTrainRes("300001");
        //showDoubleArrays(pat_test);
    }



    //获取病人信息
    public string[,] getPatientInfos()
    {
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        string[,] res;
        res = new string[1, 1];
        //res[0, 0] = "null";
        mysql.OpenSql();
        string querySql = "select pat.pt_id, pat.pt_name, pat.pt_sex, doc.dc_id, doc.dc_name, doc.dc_pro from pat, doc where pat.pt_dcID = doc.dc_id order by doc.dc_id";
        DataSet ds = mysql.SimpleSql(querySql);
        int row_num = ds.Tables[0].Rows.Count;
        if (row_num != 0)
        {
            int col_num = ds.Tables[0].Rows[0].ItemArray.Length;
            res = new string[row_num, col_num];
            for (int i = 0; i < row_num; i++)
            {
                for (int j = 0; j < col_num; j++)
                {
                    //Debug.Log("content: " + ds.Tables[0].Rows[i][j]);
                    res[i, j] = ds.Tables[0].Rows[i][j].ToString();
                }
            }
        }
        else
        {
            res[0, 0] = "null";
        }
        return res;
    }

    //显示病人的信息
    public void showDoubleArrays(string [,] infos)
    {
        int row = infos.GetLength(0);
        int col = infos.GetUpperBound(infos.Rank - 1) + 1;
        Debug.Log("res rows: " + row);
        Debug.Log("res cols: " + col);
        if (infos[0, 0] == "null")
        {
            Debug.Log("为空");
        }
        else
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Debug.Log(infos[i, j]);
                }
            }
        }
    }

    //查看病人训练方案
    public string [,] getPatientPlans(string pat_id)
    {
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        string[,] res;
        res = new string[1, 1];
        mysql.OpenSql();
        string querySql = "select act.ac_id, act.ac_name, trp.trp_hand, trp.trp_num, trp.trp_time, trp.trp_totl, trp.trp_finish " +
            "from trp, act where trp.trp_actID = act.ac_id and trp.trp_ptID = '" + pat_id + "'";
        DataSet ds = mysql.SimpleSql(querySql);
        int row_num = ds.Tables[0].Rows.Count;
        if (row_num != 0)
        {
            int col_num = ds.Tables[0].Rows[0].ItemArray.Length;
            res = new string[row_num, col_num];
            for (int i = 0; i < row_num; i++)
            {
                for (int j = 0; j < col_num; j++)
                {
                    //Debug.Log("content: " + ds.Tables[0].Rows[i][j]);
                    res[i, j] = ds.Tables[0].Rows[i][j].ToString();
                }
            }
        }
        else
        {
            res[0, 0] = "null";
        }
        return res;

    }

    //查看病人训练结果
	public string[,] getPatientTrainRes(string pat_id)
    {
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        string[,] res;
        res = new string[1, 1];
        mysql.OpenSql();
        string querySql = "select rec.rec_date, act.ac_id, act.ac_name, rec.rec_hand, rec.rec_link, rec.rec_test " +
            "from rec, act where rec.rec_actID = act.ac_id and rec.rec_ptID = '" + pat_id + "'";
        DataSet ds = mysql.SimpleSql(querySql);
        int row_num = ds.Tables[0].Rows.Count;
        if (row_num != 0)
        {
            int col_num = ds.Tables[0].Rows[0].ItemArray.Length;
            res = new string[row_num, col_num];
            for (int i = 0; i < row_num; i++)
            {
                for (int j = 0; j < col_num; j++)
                {
                    //Debug.Log("content: " + ds.Tables[0].Rows[i][j]);
                    res[i, j] = ds.Tables[0].Rows[i][j].ToString();
                }
            }
        }
        else
        {
            res[0, 0] = "null";
        }
        return res;
    }
}
