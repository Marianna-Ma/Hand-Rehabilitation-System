using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyReplay
{
    public class ReplayEntity 
    {
        //存入体名称
        public string templateName;
        //存入体索引
        public int entityIndex;
        //存入对象
        public GameObject entityTarget;

        //同步内容
        protected Queue<ReplayObjectState> ReadyToReplayData;
        protected Stack<ReplayObjectState> ActiveReplayDataStackPool;

        //优化GC
        //Color
        protected Vector3 cacheForPosition = new Vector3(100000.0f, 0f, 0f);
        protected Vector3 cacheForRotation = new Vector3(100000.0f, 0f, 0f);
        protected Transform tra;
        protected ReplayObjectState cacheState = new ReplayObjectState();
        //再输入新的状态前初始化
        public virtual void InitReplayEntity(GameObject obj, string templateName, int entityIndex) {
            this.entityTarget = obj;
            this.tra = this.entityTarget.transform;
            this.templateName = templateName;
            this.entityIndex = entityIndex;
            Debug.Log("Index is" + entityIndex);
        }
        //为重播做准备
        public virtual void PrepareForReplay(GameObject obj) {
            this.entityTarget = obj;
            this.tra = this.entityTarget.transform;

            //初始化重播数据
            if (this.ActiveReplayDataStackPool == null)
                this.ActiveReplayDataStackPool = new Stack<ReplayObjectState>();
            if (this.ReadyToReplayData == null)
                this.ReadyToReplayData = new Queue<ReplayObjectState>();
            this.ActiveReplayDataStackPool.Clear();
            this.ReadyToReplayData.Clear();

            if (obj == null) {
                Debug.LogError("@error for preparefor replay game object!");
            }
        }
        //获取同步的数据种类
        public virtual int GetChangePropertiesType() {
            int saveType = 0;
            if (!this.cacheForPosition.Equals(this.tra.localPosition)) {
                this.cacheForPosition = this.tra.localPosition;
                saveType |= (1 << ((int)SaveTargetPropertiesType.Position));
            }
            if (!this.cacheForRotation.Equals(this.tra.localEulerAngles)) {
                this.cacheForRotation = this.tra.localEulerAngles;
                saveType |= (1 << (int)SaveTargetPropertiesType.Rotation);
            }
            return saveType;
        }

        //保存新的状态
        public void InsertNewEntityState(int timePos) {
            SaveReplayEntityState(timePos);
        }
        protected virtual void SaveReplayEntityState(int timePos) {
            this.cacheState.ResetState();
            int saveType = this.GetChangePropertiesType();
            string saveStateData = ReplayObjectState.SaveCurStateProperties(this.entityIndex, timePos, tra, null, saveType);
            if (saveType != 0) {
                RecordScript.instance.SaveDataToLocalFile(saveStateData);
            }
        }

        //和本地保存文件同步内容状态
        public void SynchronizeEntity(int timePosititon) {
            if (this.ReadyToReplayData.Count <= 0)
                return;
            //当前状态读取文件位置
            ReplayObjectState curState = this.ReadyToReplayData.Peek();
            if (curState.TimePos == timePosititon) {
                //出列
                curState = this.ReadyToReplayData.Dequeue();
                this.RealSynchronizeEntity(curState);
                this.ActiveReplayDataStackPool.Push(curState);
            }
        }
        //实际同步的内容
        protected virtual void RealSynchronizeEntity(ReplayObjectState newState) {
            newState.SynchronizeProperties(this.tra);
        }
        //分析状态数据到队列
        public void ParsingStateData(int timePos, string parsingData) {
            ReplayObjectState newState = this.GetUnUsedState();
            newState.ParsingProperties(this.entityIndex, timePos, parsingData);
            this.ReadyToReplayData.Enqueue(newState);
            
        }
        private ReplayObjectState GetUnUsedState() {
            ReplayObjectState objectStateData = null;
            if (this.ActiveReplayDataStackPool.Count > 0)
                objectStateData = this.ActiveReplayDataStackPool.Pop();
            else
                objectStateData = new ReplayObjectState();
            return objectStateData;
        }
    }
    public class ReplayPlayerEntity : ReplayEntity {
        public string splineJson = string.Empty;
        public string splineAni = string.Empty;
        public string splineAsset = string.Empty;

        
    }
}
