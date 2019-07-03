using Leap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePatientHandData : MonoBehaviour {
    
    public int frame_index;
    public int frameNum = 100;

    GameObject hd;
    SaveHandData handData;
    bool allowSaveData=true;

    void Awake()
    {
        //string next_id = getActionId();
        //Debug.Log("neext iiiid: " + next_id);

        //-----------------------存数据--------------------------
        //handData = new SaveHandData(200, "/Datas/test03.json");
        //TODO 左右手分开存储时确定方法

        //hd = new GameObject();
        //hd.AddComponent<SaveHandData>();
        //handData = (SaveHandData)hd.GetComponent(typeof(SaveHandData));
        //handData.Init(200, "/Datas/test03.json");
        //PlayerPrefs.SetString("patientHandDataPath", handData.savedDataPath);
        //Debug.Log("Ready to save data:"+ handData.savedDataPath);
        allowSaveData = true;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && allowSaveData)
        {
            hd = new GameObject();
            hd.AddComponent<SaveHandData>();
            handData = (SaveHandData)hd.GetComponent(typeof(SaveHandData));
            handData.Init(200, "/Datas/test03.json");
            PlayerPrefs.SetString("patientHandDataPath", handData.savedDataPath);
            Debug.Log("Ready to save data:" + handData.savedDataPath);
            allowSaveData = false;
        }
        if (!allowSaveData)
        {
            if (handData.IsSaveCompleted())
            {
                Debug.Log("Save complete detected");
            }
        }
    }
}
