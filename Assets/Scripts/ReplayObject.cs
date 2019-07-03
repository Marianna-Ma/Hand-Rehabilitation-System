using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace LReplay
{
    public class ReplayObject
    {
        //记录位置
        public Vector3 vectorPos;

        public int timePos;

        //时间点位置信息变为字符串
        public static string TimePositionToString(int timePos,Transform target) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0};", timePos);
            sb.AppendFormat("{0},{1},{2}|",
                target.transform.localPosition.x,
                target.transform.localPosition.y,
                target.transform.localPosition.z);
            //Debug.Log(sb.ToString());
            return sb.ToString();
        }
        
        //分割字符串根据“|”分割
        void StringToPosition(string tarString) {
            string[] Contans = tarString.Split('|');
            this.StringToPositionXYZ(Contans[0]);
        }

        //分割储存位置的字符串根据“，”分割
        void StringToPositionXYZ(string tarString) {
            if (string.IsNullOrEmpty(tarString))
                return;
            string[] Pos = tarString.Split(',');
            this.vectorPos.x = float.Parse(Pos[0]);
            this.vectorPos.y = float.Parse(Pos[1]);
            this.vectorPos.z = float.Parse(Pos[2]);
        }

        //分割字符串“；”得到时间及位置字符串
        public void  GetTimeAndPosition(string Str)
        {
            string[] TimeAndPos = Str.Split(';');
            timePos = int.Parse(TimeAndPos[0]);
            this.StringToPosition(TimeAndPos[1]);
        }
    }
}
