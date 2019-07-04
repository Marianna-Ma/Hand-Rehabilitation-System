using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour {
    public GameObject textField;
    public int time = 30; //此处使用数据库中读取的数据
    public int time2 = 3;
	// Use this for initialization
	void Start () {
        StartCoroutine(Count());
        GameObject.Find("CountDownThreeText").GetComponent<Text>().text = "";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Count()
    {
        while (time > 0)
        {
            GameObject.Find("TimeCountDownText").GetComponent<Text>().text = time.ToString("00");
            yield return new WaitForSeconds(1);
            time--;
        }
        TrainOver();
    }

    IEnumerator CountThree()
    {
        while (time2 > 0)
        {
            GameObject.Find("CountDownThreeText").GetComponent<Text>().text = time2.ToString();
            yield return new WaitForSeconds(1);
            time2--;
        }
        GameObject.Find("CountDownThreeText").GetComponent<Text>().text = "开始测评";
    }

    void TrainOver()
    {
        //GameObject.Find("leftHand").SetActive(false);
       // GameObject.Find("rightHand").SetActive(false);
        GameObject.Find("TimeCountDownText").GetComponent<Text>().text = "训练完成";
    }

    public void CountDownThree()
    {
        GameObject.Find("TimeCountDownText").SetActive(false);
        StartCoroutine(CountThree());
        GameObject.Find("CountDownThreeText").GetComponent<Text>().text =time2.ToString();

    }
}
