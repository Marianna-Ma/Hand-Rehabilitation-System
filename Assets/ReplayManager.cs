using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyReplay
{
    public class ReplayManager : MonoBehaviour
    {
        //当前动画在本地重播
        public List<ReplaySaveData> curLocalAnimationReplayData;
        //当前系统状态
        private ReplaySystemState curReplaySystemState = ReplaySystemState.None;

        private RecordScript recordController = null;
        private PlayScript playController = null;

        private GameObject playerPrefab;
        private GameObject monsterPrefab;

        private const int entityParenBeginIndex = 10000;
        private int curEntityParentBeginIndex = 10000;

        public int CurEntityParentBeginIndex {
            get { return this.curEntityParentBeginIndex; }
            set { this.curEntityParentBeginIndex = value; }
        }

        private static ReplayManager instance;

        public static ReplayManager GetInstance {
            get { return instance; }
            private set { }
        }

        private void Awake()
        {
            instance = this;
            this.InitRepalySystem();
        }

        private void InitRepalySystem() {
            this.recordController = this.GetComponent<RecordScript>();
            this.playController = this.GetComponent<PlayScript>();
        }

        public void StartRecording() {
            if (this.playController.IsReplaying) {
                Debug.LogWarning("系统正在重放，无法录制");
                return;
            }
            StartCoroutine(PrepareToRecord());
        }

        private IEnumerator PrepareToRecord() {
            //开的时候设置内容索引
            this.CurEntityParentBeginIndex = entityParenBeginIndex;
            //加载角色，清理动画
            AnimationStage.GetInstance.ClearAnimationStage();
            Transform stageRoot = AnimationStage.StageRoot.transform;
            //确保摧毁所有子物体
            yield return new WaitForEndOfFrame();

            GameObject player = this.GetNewPlayerTemplate();
            this.AddChildToTarget(stageRoot, player.transform, Vector3.zero);

            player = this.GetNewPlayerTemplate();
            this.AddChildToTarget(stageRoot, player.transform, new Vector3(-300, 0, 0));

            player = this.GetNewMonsterTemplate();
            this.AddChildToTarget(stageRoot, player.transform, new Vector3(300, 0, 0));

            this.recordController.StartRecoding();

        }

        public void SaveRecordToFile() {
            if (this.playController.IsReplaying) {
                Debug.LogWarning("系统正在重播无法保存文件");
                return;
            }
            this.recordController.StopRecording();
        }

        public void LoadRecordFormFile() {
            if (this.recordController.IsRecording) {
                Debug.LogWarning("系统正在记录无法加载播放文件");
                return;
            }
            StartCoroutine(this.PrepareToReplay());
        }

        private IEnumerator PrepareToReplay() {
            AnimationStage.GetInstance.ClearAnimationStage();
            yield return new WaitForEndOfFrame();
            this.playController.LoadReplayDataFromFile();

        }

        

       

        private GameObject GetNewPlayerTemplate() {
            if (this.playerPrefab == null)
                this.playerPrefab = Resources.Load<GameObject>("AnimationPrefab/" + "Monster");
            return GameObject.Instantiate(this.playerPrefab);
        }

        private GameObject GetNewMonsterTemplate() {
            if (this.monsterPrefab == null)
                this.monsterPrefab = Resources.Load<GameObject>("AnimationPrefab/" + "Player");
            return GameObject.Instantiate(this.monsterPrefab);
        }
        private void AddChildToTarget(Transform traParent, Transform traTarget, Vector3 targetPos) {
            traTarget.parent = traParent;
            traTarget.localScale = Vector3.one;
            traTarget.localPosition = targetPos;
            traTarget.rotation = Quaternion.identity;
        }
    }
}
