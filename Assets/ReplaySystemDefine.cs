using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyReplay
{
    public class ReplaySystemDefine
    {
        public static string strEntityStateSaveFileName = "entityStateDate.txt";
        public static string strReplayProcessFileName = "replayProcess.json";

        //获取内容文件保存路径
        public static string GetStrEntityProcessFilePath() {
            string fileName = string.Empty;
            fileName = Application.dataPath + ReplaySystemDefine.strEntityStateSaveFileName;
            return fileName;
        }
        //获取重播进程文件路径
        public static string GetStrReplayProcessFilePth() {
            string fileName = string.Empty;
            fileName = Application.dataPath + ReplaySystemDefine.strReplayProcessFileName;
            return fileName;
        }

    }
    public enum ReplaySystemState
    {
        None = 0,
        Recording,
        Repalying
    }
    public enum ReplayEntityType {
        None,
        Player
    }

    //保存对象的内容状态
    public enum SaveTargetPropertiesType
    {
        None = 0,
        Rotation,
        Color,
        Position
    }
}
