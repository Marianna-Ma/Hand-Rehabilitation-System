using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyReplay
{
    public class ReplayProgress 
    {
        //存放所有记录重放实体
        public List<ReplayEntity> allEntity;
        //音频路径
        public string audioURL = string.Empty;
        //进程间隔
        public float perRecordInterval = 0.5f;
        //最大时间点
        public int maxTimePosition=0;
        //文件总长度
        public long totalSaveDataLineCount = 0;

        //保存重播的内容到字典
        [Newtonsoft.Json.JsonIgnoreAttribute]
        public Dictionary<int, ReplayEntity> allEntityDic = new Dictionary<int, ReplayEntity>();

        //重置进程
        public void ResetReplayProgress() {
            if (this.allEntity == null)
                this.allEntity = new List<ReplayEntity>();
            this.allEntity.Clear();
            this.allEntityDic.Clear();
            this.audioURL = string.Empty;
        }
        //添加新的重播内容
        public void AddNewReplayEntity(ReplayObject replayObj) {
            ReplayEntity newEntity = this.CreateNewReplayEntity(replayObj.entityType);
            newEntity.InitReplayEntity(replayObj.gameObject, string.Empty, replayObj.entityIndex);
            this.allEntity.Add(newEntity);
        }
        //添加新的父内容
        public void AddNewReplayParentEntity(ReplayParentObject replayObject, int parentIndex) {
            ReplayEntity newEntity = this.CreateNewReplayEntity(replayObject.entityType);
            newEntity.InitReplayEntity(replayObject.gameObject, replayObject.prefabName, parentIndex);
            this.allEntity.Add(newEntity);
        }
       

        //创建新的重放内容
        private ReplayEntity CreateNewReplayEntity(ReplayEntityType type) {
            ReplayEntity newEntity = null;
            if (type == ReplayEntityType.Player)
            {
                newEntity = new ReplayPlayerEntity();
            }
            else {
                newEntity = new ReplayEntity();
            }
            return newEntity;
        }
        //保存内容状态到本地文件
        public void InsertEntityNewState(int timePos) {
            for (int i = 0; i < this.allEntity.Count; i++)
                this.allEntity[i].InsertNewEntityState(timePos);
        }
        //分析状态文件
        public int PrasingStateData(string newDate) {
            int timePos = -1;
            //使用有效的方法来分离和分析数组
            string[] strDatas = newDate.Split(';');
            int entityIndex = int.Parse(strDatas[0]);
            if (this.allEntityDic.ContainsKey(entityIndex)) {
                timePos = int.Parse(strDatas[1]);
                this.allEntityDic[entityIndex].ParsingStateData(timePos, strDatas[2]);
            }
            else
                Debug.Log("@ entity dictionary has no entityIndex:" + entityIndex);
            return timePos;
        }
    }
}
