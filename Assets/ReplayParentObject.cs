using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyReplay
{
    public class ReplayParentObject : MonoBehaviour
    {
        public string prefabName;
        public ReplayEntityType entityType;
        public Dictionary<int, ReplayObject> allReplayObjects;

        private int entityBeginIndex = 10000;
        private void Start()
        {
            this.CheckSetting();
        }
        private void CheckSetting() {
            ReplayParentObject[] parentObjs = this.GetComponentsInChildren<ReplayParentObject>();
            if (parentObjs.Length > 1)
                Debug.Log("more than one Replay Parent Object script.");
        }
        public void InitReplayParentObject(int beginIndex)
        {
            this.entityBeginIndex = beginIndex + 1;
            ReplayObject[] objects = this.GetComponentsInChildren<ReplayObject>();
            if (this.allReplayObjects == null)
                this.allReplayObjects = new Dictionary<int, ReplayObject>();
            this.allReplayObjects.Clear();
            for (int i = 0; i < objects.Length; i++)
            {
                ReplayObject replayObject = objects[i];
                replayObject.entityIndex = (entityBeginIndex++);
                if (this.allReplayObjects.ContainsKey(replayObject.entityIndex))
                    Debug.LogError("Repaly object has same entity index:" + replayObject.entityIndex);
                this.allReplayObjects[replayObject.entityIndex] = replayObject;
                Debug.Log("replay obj name is" + replayObject.gameObject.name + "index is" + replayObject.entityIndex);
            }
        }
        public ReplayObject GetReplayObject(int entityIndex) {
            if (this.allReplayObjects.ContainsKey(entityIndex))
                return this.allReplayObjects[entityIndex];
            return null;
        }
    } 
}
