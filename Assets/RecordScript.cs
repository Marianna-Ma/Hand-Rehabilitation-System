using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

namespace MyReplay
{
    public class RecordScript : MonoBehaviour
    {
        //消失时间
        private float timeElapsed = 0.0f;
        //记录间隔
        private float perRecordInterval = 0.0f;
        //开始记录时间
        private float startRecordTime = 0.0f;

        //是否在记录
        private bool isRecording = false;
        //是否开始记录
        private bool isStartRecording = false;

        public bool IsRecording
        {
            get
            {
                return (this.isRecording || this.isStartRecording);
            }
        }
        //当前时间点
        private int curTimePos = 0;

        private ReplayProgress progressController;
        private StreamWriter streamWriter;

        //最大记录时间
        private int maxTimePosition = 5400;
        public static RecordScript instance;
        private void Awake()
        {
            instance = this;
        }

        //保存文件到本地
        public void SaveDataToLocalFile(string strToSave) {
            streamWriter.WriteLine(strToSave);
            this.progressController.totalSaveDataLineCount++;
        }
        //开始记录
        public void StartRecoding() {
            string fileName = ReplaySystemDefine.GetStrEntityProcessFilePath();
            //打开文件写入内容状态
            FileInfo fileInfo = new FileInfo(fileName);
            streamWriter = fileInfo.CreateText();

            this.isRecording = true;
            this.isStartRecording = true;
            this.timeElapsed = 0.0f;
            this.perRecordInterval = 0.033f;
            this.curTimePos = 0;
            this.startRecordTime = Time.realtimeSinceStartup;
            //
        }
        //停止记录
        public void StopRecording() {
            this.isRecording = false;
            this.isStartRecording = false;
            Debug.Log("停止记录保存文件");
            this.SaveRecordDataToLocalFile();
        }
        private void SaveRecordDataToLocalFile() {
            streamWriter.Flush();
            streamWriter.Close();
            this.SaveFile();
        }
        private void SaveFile() {
            Debug.Log("开始时间" + Time.realtimeSinceStartup);
            string fileName = ReplaySystemDefine.GetStrReplayProcessFilePth();
            Debug.Log("保存文件到：" + fileName);

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(this.progressController, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
            FileInfo fileInfo = new FileInfo(fileName);
            using (StreamWriter sw = fileInfo.CreateText()) {
                sw.Write(jsonStr);
            }
            Debug.Log("结束时间" + Time.realtimeSinceStartup);

            stopWatch.Stop();
            TimeSpan timeSpan = stopWatch.Elapsed;
            double takeTime = timeSpan.TotalSeconds;
            double takeTime2 = timeSpan.TotalMilliseconds;

            Debug.Log("测试保存文件所用时间：" + takeTime + "当前时间:" + takeTime2);

            //打印信息
            
            
        }
        //初始化记录进程
        private void InitRecordProgress() {
            if (this.progressController == null)
                this.progressController = new ReplayProgress();
            this.progressController.audioURL = "helloWord.mp3";

            //重置重播进程的控制者
            GameObject[] allReplayObjs = GameObject.FindGameObjectsWithTag("MyPlayer");
            for (int i = 0; i < allReplayObjs.Length; i++) {
                ReplayParentObject replayParent = allReplayObjs[i].GetComponent<ReplayParentObject>();
                if (replayParent == null)
                {
                    Debug.LogError(" replay parent not have ReplayParentObject script.");
                    continue;
                }
                int parentIndex = ReplayManager.GetInstance.CurEntityParentBeginIndex;
                replayParent.InitReplayParentObject(parentIndex);
                this.progressController.AddNewReplayParentEntity(replayParent, parentIndex);
                ReplayManager.GetInstance.CurEntityParentBeginIndex = (parentIndex + 10000);

                Debug.Log("下一个重放父物体的索引" + ReplayManager.GetInstance.CurEntityParentBeginIndex);
                foreach (var replayChildObject in replayParent.allReplayObjects)
                    this.progressController.AddNewReplayEntity(replayChildObject.Value);

                this.progressController.InsertEntityNewState(this.curTimePos);
            } 
        }

        private void Update()
        {
            if (!this.isRecording)
                return;
            if (Time.realtimeSinceStartup - this.startRecordTime >= this.perRecordInterval) {
                this.curTimePos++;
                this.progressController.InsertEntityNewState(this.curTimePos);
                this.startRecordTime = Time.realtimeSinceStartup;
                this.progressController.maxTimePosition = this.curTimePos;
                if (this.curTimePos >= maxTimePosition)
                    this.StopRecording();
                
            }
        }

        //分析
        public void PauseRecording()
        {
            this.isRecording = false;
        }

        public void ResumeRecording()
        {
            this.isRecording = true;
        }


    }
}
