using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTable : MonoBehaviour {

    public GameObject PatientData_Prefab;//表头预设

    void Start()
    {
       
    }


	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        /*
        GameObject table = GameObject.Find("Canvas/DoctorCheckPatientPanel/ScrollView/Viewport/Content");
        GameObject patientdata = GameObject.Find("Canvas/DoctorCheckPatientPanel/ScrollView/Viewport/Content/PatientData");
        patientdata.SetActive(false);
        for (int i = 0; i < 20; i++)//添加并修改预设的过程，将创建10行
        {
            //Debug.Log(i);
            //在Table下创建新的预设实例
            //GameObject table = GameObject.Find("Canvas/DoctorCheckPatientPanel/ScrollView/Viewport/Content");
            //Debug.Log(table.name);
            GameObject row = GameObject.Instantiate(PatientData_Prefab, table.transform.position, table.transform.rotation) as GameObject;
            row.name = "row" + (i + 1);
            row.transform.SetParent(table.transform);
            row.transform.localScale = Vector3.one;//设置缩放比例1,1,1，不然默认的比例非常大
            //设置预设实例中的各个子物体的文本内容
            row.transform.Find("PatientID").GetComponent<Text>().text = "ID:" + (i + 1);
            row.transform.Translate(0, i * 30, 0);
        }
        */
    }
}
