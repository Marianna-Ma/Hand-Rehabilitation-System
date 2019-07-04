using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlanTable : MonoBehaviour {

    public GameObject PlanData_Prefab;//表头预设

    void Start()
    {
        GameObject table = GameObject.Find("ScrollView/Viewport/Content");
        GameObject plandata = GameObject.Find("ScrollView/Viewport/Content/ActionData");
        plandata.SetActive(false);
        for (int i = 0; i < 20; i++)//添加并修改预设的过程，将创建10行
        {
            //Debug.Log(i);
            //在Table下创建新的预设实例
            GameObject row = GameObject.Instantiate(PlanData_Prefab, table.transform.position, table.transform.rotation) as GameObject;
            row.name = "plan" + (i + 1);
            row.transform.SetParent(table.transform);
            row.transform.localScale = Vector3.one;//设置缩放比例1,1,1，不然默认的比例非常大
            //设置预设实例中的各个子物体的文本内容
            row.transform.Find("ActionName").GetComponent<Text>().text = "动作" + (i + 1);
            row.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}
