using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TrainingPlan{
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
    //封装好的数据库类

    public static void addTrainingPlan(string trp_ptID, string trp_actID, int trp_hand, int trp_num, int trp_time, int trp_totl)
    {
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "insert into trp(trp_ptID, trp_actID, trp_hand, trp_num, trp_time, trp_totl) values(";
        querySql = querySql + "'" + trp_ptID + "', '" + trp_actID + "', '" + trp_hand + "', '" + trp_num + "', '" + trp_time + "', '" + trp_totl + "')";
        Debug.Log(querySql);
        mysql.SimpleSql(querySql);
        mysql.Close();
    }

    public static string[] deleteTrainingPlan(string trp_ptID, string trp_actID, int trp_hand)
    {
        string[] res = new string[1];
        res[0] = "null";
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "select trp_ptID, trp_actID, trp_hand from trp where trp_ptID = '" 
            + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand +"'";
        DataSet ds = mysql.SimpleSql(querySql);
        if (ds.Tables[0].Rows.Count != 0)
        {
            res = new string[3];
            res[0] = ds.Tables[0].Rows[0][0].ToString();
            res[1] = ds.Tables[0].Rows[0][1].ToString();
            res[2] = ds.Tables[0].Rows[0][2].ToString();
            querySql = "delete from trp where trp_ptID = '" + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
            Debug.Log(querySql);
            mysql.SimpleSql(querySql);
        }
        mysql.Close();
        return res;
    }

    public static DataSet findTrainingPlan(string trp_ptID, string trp_actID, int trp_hand)
    {
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "select * from trp where trp_ptID = '" 
            + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
        DataSet ds = mysql.SimpleSql(querySql);
        mysql.Close();
        return ds;
    }
    /// <summary>
    /// 查询未完成的训练计划
    /// </summary>
    public static DataSet findUnfinishedTrainingPlan(string trp_ptID)
    {
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "select * from trp where trp_ptID = '" + trp_ptID + "' and trp_totl > trp_finish";
        DataSet ds = mysql.SimpleSql(querySql);
        mysql.Close();
        return ds;
    }

    /// <summary>
    /// 更改训练计划，只能改次数、时长、总天数
    /// </summary>
    public static void changeTrainingPlan(string trp_ptID, string trp_actID, int trp_hand, int trp_num, int trp_time, int trp_totl)
    {
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "select trp_ptID, trp_actID, trp_hand from trp where trp_ptID = '" 
            + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
        DataSet ds = mysql.SimpleSql(querySql);
        if (ds.Tables[0].Rows.Count != 0)
        {
            querySql = "update trp set trp_num='"+ trp_num + "' and trp_time='"+ trp_time + "' and trp_totl='"+ trp_totl + 
                "' where trp_ptID = '" + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
            Debug.Log(querySql);
            mysql.SimpleSql(querySql);
        }
        mysql.Close();
    }

    public static void finishOnceTrainingPlan(string trp_ptID, string trp_actID, int trp_hand)
    {
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "select trp_ptID, trp_actID, trp_hand from trp where trp_ptID = '"
            + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
        DataSet ds = mysql.SimpleSql(querySql);
        if (ds.Tables[0].Rows.Count != 0)
        {
            querySql = "update trp set trp_alnum=trp_alnum+1 where trp_ptID = '"
                + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
            Debug.Log(querySql);
            mysql.SimpleSql(querySql);
        }
        mysql.Close();
    }

    public static void finishOneDayTrainingPlan(string trp_ptID, string trp_actID, int trp_hand)
    {
        MySqlAccess mysql = new MySqlAccess(host, port, userName, password, databaseName);
        mysql.OpenSql();
        string querySql = "select trp_ptID, trp_actID, trp_hand from trp where trp_ptID = '" 
            + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
        DataSet ds = mysql.SimpleSql(querySql);
        if (ds.Tables[0].Rows.Count != 0)
        {
            querySql = "update trp set trp_finish=trp_finish+1,trp_alnum=0 where trp_ptID = '"
                + trp_ptID + "' and trp_actID = '" + trp_actID + "' and trp_hand = '" + trp_hand + "'";
            Debug.Log(querySql);
            mysql.SimpleSql(querySql);
        }
        mysql.Close();
    }
}
