using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using System.IO;


public class StdActionLibraryUI : MonoBehaviour, IPointerClickHandler {
    public InputField actionNameInput;
    public InputField actionPicPath;

    Text stdActionID;
    string patient_id;


	// Use this for initialization
	void Start () {
        patient_id = "400002";
        GameObject.Find("StdActionLibraryID").GetComponent<Text>().text = patient_id;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerPress.name == "Button_AddPicPath")
        {
            GameObject path = GameObject.Find("Input_StdActionPath");
            OpenFileName openFileName = new OpenFileName();

            openFileName.structSize = Marshal.SizeOf(openFileName);
            //openFileName.filter = "文件(*." + type + ")\0*." + type + "";
            openFileName.filter = "文件(*.png;*.jpg)\0*.png;*.jpg";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            openFileName.title = "选择文件";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

            if (FileLocalDialog.GetSaveFileName(openFileName))//点击系统对话框框保存按钮
            {
                //TODO
                //Debug.Log(openFileName.file);
                path.GetComponent<InputField>().text = openFileName.file;
                Debug.Log("动作路径： " + actionPicPath.text.ToString());
                //Debug.Log(openFileName.fileTitle);
            }

        }
        if(eventData.pointerPress.name == "Button_Add")
        {
            Debug.Log("动作名： " + actionNameInput.text.ToString());
            Debug.Log("动作图片路径" + actionPicPath.text.ToString());
            StandardActionLibrary stdlibrary = new StandardActionLibrary();
            stdlibrary.addStandardAction(patient_id, actionNameInput.text.ToString(), actionPicPath.text.ToString());

            Messagebox.MessageBox(IntPtr.Zero, "添加标准动作成功！", "成功", 0);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
