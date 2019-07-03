using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using System;

public class DataControlledRightHand : handData {
    //LeapProvider provider1;

    GameStatus status;
    int frameNo,dataLength;
    DateTime now_Time;
    DateTime old_Time;
    double delta24;

    // Use this for initialization
    void Start () {
        //provider1 = FindObjectOfType<LeapProvider>() as LeapProvider;

        status = LoadJson.LoadJsonFromFile(defaultPath);
        frameNo = 0;
        dataLength = status.frame_data.Length;
        old_Time = DateTime.Now;
        delta24 = 1.0 / 24.0;
    }
	
	// Update is called once per frame
	void Update () {
        //Frame frame = provider1.CurrentFrame;
        //SaveOverallHandData(frame);
        //SaveFingerFrame(frame);
        now_Time = DateTime.Now;
        if ((now_Time - old_Time).TotalSeconds >= delta24)
        {
            ReadJsonByFrame(frameNo, status);
            moveRightHand();
            frameNo++;
            old_Time = now_Time;
            if (frameNo == dataLength) frameNo = 0;
        }
        
    }

    void moveRightHand()
    {

        transform.position = m_hand_position[1].hand_position.ToVector3();
        transform.rotation = Quaternion.Euler(m_hand_position[1].hand_rotation.ToVector3());

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.name == "thumb_1")
            {
                t.transform.rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_THUMB].prox0.joint_rotation.ToVector3());
                t.transform.Rotate(new Vector3(-90, 0, 0));
                t.transform.Rotate(new Vector3(0, -90, 0));
                t.GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_THUMB].inter0.joint_rotation.ToVector3());
                t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).transform.Rotate(new Vector3(0, -90, 0));
            }
            if (t.name == "index_finger_1")
            {
                t.transform.rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_INDEX].prox0.joint_rotation.ToVector3());
                t.transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_INDEX].inter0.joint_rotation.ToVector3());
                t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_INDEX].dist0.joint_rotation.ToVector3());
                t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
            }
            if (t.name == "middle_finger_1")
            {
                t.transform.rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_MIDDLE].prox0.joint_rotation.ToVector3());
                t.transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_MIDDLE].inter0.joint_rotation.ToVector3());
                t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_MIDDLE].dist0.joint_rotation.ToVector3());
                t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
            }
            if (t.name == "ring_finger_1")
            {
                t.transform.rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_RING].prox0.joint_rotation.ToVector3());
                t.transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_RING].inter0.joint_rotation.ToVector3());
                t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_RING].dist0.joint_rotation.ToVector3());
                t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
            }
            if (t.name == "pinky_1")
            {
                t.transform.rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_PINKY].prox0.joint_rotation.ToVector3());
                t.transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_PINKY].inter0.joint_rotation.ToVector3());
                t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                t.GetChild(0).GetChild(0).rotation = Quaternion.Euler(m_FingerDatas[1][Finger.FingerType.TYPE_PINKY].dist0.joint_rotation.ToVector3());
                t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
            }
        }
    }
}
