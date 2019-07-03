using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using System.Data;

public class TestLeftHand : MonoBehaviour {
    LeapProvider provider;
    List<Hand> leftHands;
    //public GameObject ob;

    //IP地址
    public string host;
    //端口号
    public string port;
    //用户名
    public string userName;
    //密码
    public string password;
    //数据库名称
    public string databaseName;
    //封装好的数据库类
    MySqlAccess mysql;

    // Use this for initialization
    void Start () {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        leftHands = new List<Hand>();

        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        //testMysql();
    }

    void testMysql()
    {
        DataSet ds = mysql.SimpleSql("Select * from act");
        if (ds != null)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables[0].Rows[i].ItemArray.Length; j++)
                {
                    Debug.Log("Result" + i + ":" + ds.Tables[0].Rows[i][j]);
                }
            }
        }
        else Debug.Log("无结果");
        mysql.Close();
    }
	
	// Update is called once per frame
	void Update () {
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                leftHands.Add(hand);
            }
            if (leftHands.Count != 0) break;
        }

        if (leftHands.Count != 0)
        {
            Hand hand = leftHands[0];

            transform.position = hand.WristPosition.ToVector3();
            transform.rotation = hand.Basis.CalculateRotation();

            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.name == "thumb_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_THUMB)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.transform.Rotate(new Vector3(0, -90, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).transform.Rotate(new Vector3(0, -90, 0));
                        }
                    }
                }
                if (t.name == "index_finger_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_INDEX)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
                if (t.name == "middle_finger_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_MIDDLE)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
                if (t.name == "ring_finger_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_RING)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
                if (t.name == "pinky_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_PINKY)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
            }
        }
        else
        {
            transform.position = new Vector3(10, 10, -20);
        }
        leftHands.Clear();
    }
}

