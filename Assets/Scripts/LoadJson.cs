using UnityEngine;
using System.Collections;
using System.IO;

public class LoadJson : MonoBehaviour
{
    private static string defaultPath = "/Datas/test02.json";

    public static GameStatus LoadJsonFromFile(string readPath = "/Datas/test02.json")
	{
        StreamReader sr;

        if (!readPath.Equals("/Datas/test02.json") && File.Exists(Application.dataPath + readPath))
        {
            sr = new StreamReader(Application.dataPath + readPath);
        }
        else if (File.Exists(Application.dataPath + defaultPath))
        {
            sr = new StreamReader(Application.dataPath + defaultPath);
        }
        else return null;

		//FileStream file = File.Open(Application.dataPath + "/Test.json", FileMode.Open, FileAccess.ReadWrite);
		//if (file.Length == 0)
		//{
		//    return null;
		//}

		//string json = (string)bf.Deserialize(file);
		//file.Close();

		if (sr == null)
		{
			return null;
		}
		string json = sr.ReadToEnd();

		if (json.Length > 0)
		{
			return JsonUtility.FromJson<GameStatus>(json);
		}

		return null;
	}
}
