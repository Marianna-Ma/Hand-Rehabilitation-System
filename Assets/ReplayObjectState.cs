using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace MyReplay
{
    public class ReplayObjectState
    {
        //位置和方向
        private bool changePos = false;
        private Vector3 vectorPos;
        private bool changeRot = false;
        private Vector3 vectorRot;

        //颜色
        private bool changeColor = false;
        private float r;
        private float g;
        private float b;
        private float a;  

        private int timePosition = -1;
        private int entityIndex = -1;
        public int TimePos
        {
            get { return this.timePosition; }
        }
        public int EntityIndex
        {
            get { return this.entityIndex; }
        }

        public void ResetState()
        {
            this.changeColor = false;
            this.changePos = false;
            this.changeRot = false;
            this.timePosition = -1;
            this.entityIndex = -1;
        }

        //保存当前状态内容
        public static string SaveCurStateProperties(int entityIndex, int timePos, Transform targetTra, GUITexture targetTexture, int saveType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0};{1}", entityIndex, timePos);
            int targetType = 1 << ((int)SaveTargetPropertiesType.Position);
            //&只能用于判断两个对象的对错
            if ((saveType & targetType) == targetType)
            {
                //保存位置
                sb.AppendFormat("{0},{1},{2}|",
                    targetTra.localPosition.x,
                    targetTra.localPosition.y,
                    targetTra.localPosition.z);
            }
            else
            {
                sb.Append("|");
            }
            targetType = 1 << ((int)SaveTargetPropertiesType.Rotation);
            if ((saveType & targetType) == targetType)
            {
                //保存角度
                sb.AppendFormat("{0},{1},{2}|",
                    targetTra.localEulerAngles.x,
                    targetTra.localEulerAngles.y,
                    targetTra.localEulerAngles.z);
            }
            else {
                sb.Append("|");
            }
            targetType = 1 << ((int)SaveTargetPropertiesType.Color);
            if ((saveType & targetType) == targetType)
            {
                sb.AppendFormat("{0},{1},{2},{3}|",
                    targetTexture.color.r,
                    targetTexture.color.g,
                    targetTexture.color.b,
                    targetTexture.color.a);
            }
            else {
                sb.Append("|");
            }
            return sb.ToString();

        }
        //同步内容
        public void ParsingProperties(int entityIndex, int timePos, string targetStr) {
            //最佳的垃圾回收机制
            this.timePosition = timePos;
            this.entityIndex = entityIndex;

            string[] properties = targetStr.Split('|');
            //位置
            this.ParsingPosition(properties[0]);
            //角度
            this.ParsingRotation(properties[1]);
            //颜色
            this.ParsingColor(properties[2]);
        }
        //分析位置
        private void ParsingPosition(string strPosition) {
            if (string.IsNullOrEmpty(strPosition))
                return;
            this.changePos = true;
            string[] strPos = strPosition.Split(',');
            this.vectorPos.x = float.Parse(strPos[0]);
            this.vectorPos.y = float.Parse(strPos[1]);
            this.vectorPos.z = float.Parse(strPos[2]);
        }
        //分析角度
        private void ParsingRotation(string strRotation) {
            if (string.IsNullOrEmpty(strRotation))
                return;
            this.changeRot = true;
            string[] strRot = strRotation.Split(',');
            this.vectorRot.x = float.Parse(strRot[0]);
            this.vectorRot.y = float.Parse(strRot[1]);
            this.vectorRot.z = float.Parse(strRot[2]);
        }
        //分析颜色
        private void ParsingColor(string strColor) {
            if (string.IsNullOrEmpty(strColor))
                return;
            this.changeColor = true;
            string[] strClo = strColor.Split(',');
            this.r = float.Parse(strClo[0]);
            this.g = float.Parse(strClo[1]);
            this.b = float.Parse(strClo[2]);
            this.a = float.Parse(strClo[3]);
        }
        //同步数据(位置角度)
        public void SynchronizeProperties(Transform tra) {
            if (this.changePos) {
                tra.localPosition = this.vectorPos;
            }
            if (this.changeRot) {
                tra.localEulerAngles = this.vectorRot;
            }
        }
        //同步颜色数据
        
    }
}

