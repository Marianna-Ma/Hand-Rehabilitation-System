using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace LReplay
{
    public class ReplayScript : MonoBehaviour
    {
        public int timePos=0;
        public float startTimePos=0;
        public Transform target;
        public float replayInterval;
        public bool isReplay = false;

        ReplayObject replayEntity;
        //使用队列来动态加载位置数据
        protected Queue<ReplayObject> readyToReplayData;

        private void Start()
        {
            readyToReplayData = new Queue<ReplayObject>();
            readyToReplayData.Clear();
            replayInterval = 0.02f;
        }

        private void Update()
        {
            //按下Q键回放之前的操作
            if (Input.GetKeyDown(KeyCode.R))
            {
                startTimePos = Time.realtimeSinceStartup ;
                StartCoroutine(LoadReolayDataFromFile());
                timePos = 0;
                isReplay = true;
            }
            if (isReplay) {
                if (Time.realtimeSinceStartup - this.startTimePos >= replayInterval) {
                    timePos++;
                    startTimePos = Time.realtimeSinceStartup;
                    ReplayObject curState = this.readyToReplayData.Peek();
                    if (curState.timePos == timePos) {
                        target.transform.position = curState.vectorPos;
                        curState = this.readyToReplayData.Dequeue();
                    }
                }
            }
        }
        //读取记录位置文件的信息
        IEnumerator LoadReolayDataFromFile()
        {
            string fileName = Application.dataPath + "/TextFile" + "/File.txt";
            FileInfo fileInfo = new FileInfo(fileName);
            string lineData = string.Empty;
            int dataCount = 0;
            if (fileInfo.Exists)
            {
                using (StreamReader sr = fileInfo.OpenText())
                {
                    while ((lineData = sr.ReadLine()) != null)
                    {
                        dataCount++;
                        replayEntity = new ReplayObject();
                        replayEntity.GetTimeAndPosition(lineData);

                        readyToReplayData.Enqueue(replayEntity);
                    }
                }
            }
            yield return null;
        }
    }
}
