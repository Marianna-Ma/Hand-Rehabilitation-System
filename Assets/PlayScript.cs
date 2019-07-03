using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System;

namespace MyReplay
{
    public class PlayScript : MonoBehaviour
    {
        private ReplayProgress progressController;

        private float perReplayInterval = 0.0f;
        private float startPlayTime = 0.0f;
        private int curTimePos = 0;
        private bool isReplaying = false;

        //读取需要的数据来重新播放动画
        private bool isStartReplay = false;
        //每条路线的数据数量
        private int minLoadLineDataCount = 60;
        private int startReplayTimePos = 60;
        private int firstTimePosLineDataCount = 0;
        private bool firstUpdateOver = false;

        public bool IsReplaying
        {
            get { return this.isReplaying || isStartReplay; }
        }

        public void LoadReplayDataFromFile()
        {
            StartCoroutine(this._LoadReplayDataFromFile());
        }
        IEnumerator _LoadReplayDataFromFile()
        {
            string fileName = ReplaySystemDefine.GetStrReplayProcessFilePth();
            Debug.Log("加载的文件名称：" + fileName);
            Debug.Log("开始加载的时间：" + Time.realtimeSinceStartup);

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            //加载进程文件
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                using (StreamReader sr = fileInfo.OpenText())
                {
                    string jsonProgress = sr.ReadToEnd();
                    this.progressController = Newtonsoft.Json.JsonConvert.DeserializeObject<ReplayProgress>(jsonProgress, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    this.perReplayInterval = this.progressController.perRecordInterval;
                }
            }
            else
            {
                Debug.LogError("replay file is not exist,please check it.");
                yield break;
            }
            if (this.perReplayInterval <= 0.0f)
            {
                this.perReplayInterval = 0.033f;
                Debug.Log("每一次的重放间隔小于0.0f");
            }
            int perSecondCount = Mathf.RoundToInt(1f / this.perReplayInterval);
            int entityCount = this.progressController.allEntity.Count;
            this.minLoadLineDataCount = perSecondCount * 2 * entityCount;
            this.startReplayTimePos = Mathf.Min(this.minLoadLineDataCount * 4, this.progressController.maxTimePosition * entityCount);
            this.firstTimePosLineDataCount = entityCount;
            this.firstUpdateOver = false;
            this.isReplaying = false;
            this.isStartReplay = false;
            Debug.Log("@per second count is " + perSecondCount);
            Debug.Log("@min load line data count is " + this.minLoadLineDataCount);
            Debug.Log("@start replay animation timepos is " + this.startReplayTimePos);
            Debug.Log("@first time pos line data count is " + this.firstTimePosLineDataCount);

            this.LoadEntityDataBeforeReplay();

            fileName = ReplaySystemDefine.GetStrEntityProcessFilePath();
            fileInfo = new FileInfo(fileName);
            string lineData = string.Empty;
            int dataCount = 0;
            if (fileInfo.Exists)
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((lineData = sr.ReadLine()) != null)
                    {
                        dataCount++;
                        this.PrasingStateDate(lineData, dataCount);
                        if (dataCount >= this.firstTimePosLineDataCount && !this.firstUpdateOver)
                        {
                            Debug.Log("更新第一个时间的位置");
                            this.firstUpdateOver = true;
                            for (int i = 0; i < entityCount; i++)
                            {
                                this.progressController.allEntity[i].SynchronizeEntity(0);
                            }
                        }
                        this.CheckToReplayAnimation(dataCount);
                        if (dataCount % this.minLoadLineDataCount == 0)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
            }
            else {
                Debug.LogError("replay progress data file is not exist.");
                yield break;
            }
            stopWatch.Stop();
            TimeSpan timeSpan = stopWatch.Elapsed;
            double takeTime = timeSpan.TotalSeconds;
            double takeTime2 = timeSpan.TotalMilliseconds;

            //打印程序运行的时间
            Debug.Log("@LoadRecordFile__________take time is " + takeTime + " current Time is " + takeTime2);
            Debug.Log("@end load time:" + Time.realtimeSinceStartup);
            yield return 0;
        }




        private void LoadEntityDataBeforeReplay()
        {
            Debug.Log("Animation entity count is" + this.progressController.allEntity.Count);
            List<ReplayEntity> allEntity = this.progressController.allEntity;
            StringBuilder sb = null;

            GameObject templateObject = null;
            ReplayParentObject parentScript = null;
            for (int i = 0; i < allEntity.Count; i++)
            {
                ReplayEntity replayEntity = allEntity[i];
                sb = new StringBuilder();
                sb.AppendFormat("@entity information: prefab name is {0} entityIndex is {1}.", allEntity[i].templateName, allEntity[i].entityIndex);
                Debug.Log(sb.ToString());
                this.progressController.allEntityDic[replayEntity.entityIndex] = replayEntity;

                if (replayEntity.entityIndex % 10000 == 0)
                {
                    GameObject loadedPrefab = Resources.Load<GameObject>("AnimationPrefab/" + replayEntity.templateName);
                    if (loadedPrefab != null)
                    {
                        templateObject = GameObject.Instantiate(loadedPrefab) as GameObject;
                        //UI

                        parentScript = templateObject.GetComponent<ReplayParentObject>();
                        templateObject.SetActive(true);
                        if (parentScript != null)
                            parentScript.InitReplayParentObject(replayEntity.entityIndex);
                        templateObject.transform.parent = AnimationStage.StageRoot.transform;
                        templateObject.transform.localScale = Vector3.one;
                        templateObject.transform.localPosition = Vector3.zero;
                        templateObject.transform.rotation = Quaternion.identity;
                        //同步第一次位置
                        replayEntity.PrepareForReplay(templateObject);
                    }
                    else
                        Debug.Log("加载的预制体为空");
                }
                else
                {
                    //对于子物体
                    if (parentScript == null)
                        Debug.Log("父物体脚本为空");
                    ReplayObject replayObject = parentScript.GetReplayObject(replayEntity.entityIndex);
                    if (replayEntity == null)
                        Debug.Log("播放的对象为空：" + replayEntity.entityIndex);
                    Debug.Log("重播对象的名称为：" + replayObject.name);
                    replayEntity.PrepareForReplay(replayObject.gameObject);
                }
            }
        }
        private void StartReplaying()
        {
            this.startPlayTime = Time.realtimeSinceStartup;
            this.curTimePos = 0;
            this.perReplayInterval = this.progressController.perRecordInterval;
            this.isReplaying = true;
            Debug.Log("开始播放动画");
            //
        }

        private void Update()
        {
            if (!this.isReplaying || !this.isStartReplay)
                return;
            if (this.curTimePos >= this.progressController.maxTimePosition) {
                this.OnReplayOver();
                return;
            }
            if (Time.realtimeSinceStartup - this.startPlayTime >= this.perReplayInterval) {
                this.curTimePos++;
                this.startPlayTime = Time.realtimeSinceStartup;
                int entityCount = this.progressController.allEntity.Count;
                for (int i = 0; i < entityCount; i++)
                    this.progressController.allEntity[i].SynchronizeEntity(this.curTimePos);
            }
        }

        private void PrasingStateDate(string lineStateData, int lineIndex)
        {
            int timePos = this.progressController.PrasingStateData(lineStateData);
            if ((timePos >= this.startReplayTimePos || (lineIndex >= progressController.totalSaveDataLineCount)) && this.firstUpdateOver)
            {
                this.isStartReplay = true;
                this.StartReplaying();
            }
        }
        private void CheckToReplayAnimation(int lineIndex)
        {
            if (this.firstUpdateOver && lineIndex >= this.progressController.totalSaveDataLineCount)
            {
                this.isStartReplay = true;
                this.StartReplaying();
            }
        }
        private void OnReplayOver() {
            Debug.Log("重播结束");
            this.isReplaying = false;
            this.isStartReplay = false;
            
        }
    }
}
