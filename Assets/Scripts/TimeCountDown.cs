using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour {
    public GameObject textField;
    public int time = 30; //此处使用数据库中读取的数据
	// Use this for initialization
	void Start () {
        StartCoroutine(Count());
        
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

    void TrainOver()
    {
        GameObject.Find("leftHand").SetActive(false);
        GameObject.Find("rightHand").SetActive(false);
        GameObject.Find("TimeCountDownText").GetComponent<Text>().text = "训练完成";
    }
}
