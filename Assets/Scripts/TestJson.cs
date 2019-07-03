using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace LReplay
{
    public class MyJson
    {
        public string myString = "lala";
        public int scoure = 3;
        public float scoures = 3.0f;
    }
    public class TestJson : MonoBehaviour
    {
        //时间点
        public int timePos;
        //记录的目标
        public GameObject target;
        //开始记录的时间
        public float startRecordTime;

        //txt路径名称
        private string txtFileName;
        //Json路径名称
        private string jsonFileName;
        //记录间隔
        private float recordInterval;
        //判断位置是否改变
        protected Vector3 cachePosition = new Vector3(1000.0f, 0f, 0f);


        //流文件
        private StreamWriter streamWriter;
        private MyJson myJson;
        private ReplayObject myReplayObject;

        private void Awake()
        {
            myJson = new MyJson();
        }

        private void Start()
        {
            txtFileName = Application.dataPath + "/TextFile" + "/File.txt";
            jsonFileName = Application.dataPath + "/TextFile" + "/File.json";
            CreatTxtAndJson();
            //timePos = Time.realtimeSinceStartup;
            recordInterval = 0.033f;
            startRecordTime = 2.0f;
            StartRecord();
        }
        //生成文件
        void CreatTxtAndJson()
        {
            if (!File.Exists(txtFileName))
            {
                streamWriter = File.CreateText(txtFileName);
            }
            else
            {

                Debug.Log("Txt Is Exist!");
            }
            if (!File.Exists(jsonFileName))
            {
                File.Create(jsonFileName);
            }
            else
            {
                Debug.Log("Json Is Exist!");
            }
        }
        //开始记录
        void StartRecord()
        {
            FileInfo fileInfo = new FileInfo(txtFileName);
            streamWriter = fileInfo.CreateText();
            string jsonStr = JsonConvert.SerializeObject(this.myJson, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            FileInfo fileInfo2 = new FileInfo(jsonFileName);
            using (StreamWriter sw = fileInfo2.CreateText())
            {
                sw.Write(jsonStr);
            }
        }
        //写入文件
        void WriteData(string saveData)
        {
            streamWriter.WriteLine(saveData);
        }
        //结束写入
        void Finished()
        {
            streamWriter.Flush();
            streamWriter.Close();
        }
        private void Update()
        {
            //timePos = Time.realtimeSinceStartup*100;
            string curData =string.Empty;
            if (Time.realtimeSinceStartup - startRecordTime >= recordInterval)
            {
                timePos++;
                curData = ReplayObject.TimePositionToString(timePos, target.transform);
                if (!cachePosition.Equals(target.transform.localPosition))
                {
                    WriteData(curData);
                    this.cachePosition = this.target.transform.localPosition;
                    //Debug.Log("1111111111");
                }
                this.startRecordTime = Time.realtimeSinceStartup;
            }
            //按下E键停止记录
            if (Input.GetKeyDown(KeyCode.E))
            {
                Finished();
            }
        }
    }
}
