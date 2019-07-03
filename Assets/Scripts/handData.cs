using System;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

//手部整体的位置信息与旋转信息
public struct OverallData
{
    public Vector hand_position;
    public Vector hand_rotation;

    public OverallData(Vector pos, Vector rot)
    {
        hand_position = pos;
        hand_rotation = rot;
    }

    public void Set(OverallData od)
    {
        hand_position = od.hand_position;
        hand_rotation = od.hand_rotation;
    }

    public void Set(Vector pos, Vector rot)
    {
        hand_position = pos;
        hand_rotation = rot;
    }
}

//记录的每个指节点的信息:每个指节点的信息和其对应的旋转角度
public struct FingerData
{
    public JointData meta0;
    public JointData prox0;
    public JointData inter0;
    public JointData dist0;
    public JointData dist1;

    public FingerData(FingerData f_data)
    {
        meta0 = f_data.meta0;
        prox0 = f_data.prox0;
        inter0 = f_data.inter0;
        dist0 = f_data.dist0;
        dist1 = f_data.dist1;
    }

    public FingerData(JointData m, JointData p, JointData i, JointData d0, JointData d1)
    {
        meta0 = m;
        prox0 = p;
        inter0 = i;
        dist0 = d0;
        dist1 = d1;
    }

    public FingerData(Vector m_pos, Vector m_rot, Vector p_pos, Vector p_rot, Vector i_pos,
        Vector i_rot, Vector d0_pos, Vector d0_rot, Vector d1_pos, Vector d1_rot)
    {
        meta0.joint_position = m_pos;
        meta0.joint_rotation = m_rot;
        prox0.joint_position = p_pos;
        prox0.joint_rotation = p_rot;
        inter0.joint_position = i_pos;
        inter0.joint_rotation = i_rot;
        dist0.joint_position = d0_pos;
        dist0.joint_rotation = d0_rot;
        dist1.joint_position = d1_pos;
        dist1.joint_rotation = d1_rot;
    }

    public void SetData(JointData m, JointData p, JointData i, JointData d0, JointData d1)
    {
        meta0 = m;
        prox0 = p;
        inter0 = i;
        dist0 = d0;
        dist1 = d1;
    }
}


public struct JointData
{
    public Vector joint_position;
    public Vector joint_rotation; //旋转角度

    public JointData(Vector pos, Vector rot)
    {
        joint_position = pos;
        joint_rotation = rot;
    }

    public JointData(JointData j_data)
    {
        joint_position = j_data.joint_position;
        joint_rotation = j_data.joint_rotation;
    }

    public void Set_Vector(Vector pos, Vector rot)
    {
        joint_position = pos;
        joint_rotation = rot;
    }

    public void Set_JointData(JointData j_data)
    {
        joint_position = j_data.joint_position;
        joint_rotation = j_data.joint_rotation;
    }
}

public class handData : MonoBehaviour {

    LeapProvider provider;
    const int BUFFER_MAX = 5;

    // 判断时间间隔
    int startSaveFrame;
    int first_get = 0;
    double delta;
    DateTime now_Time;
    DateTime old_Time;
    TimeSpan time_Span;
    double total_Seconds;

    //整个手的位置信息及旋转信息，[0]-左手，[1]-右手
    protected OverallData[] m_hand_position = new OverallData[2];

    //记录每个手指指关节的位置信息和旋转信息，[0]-左手，[1]-右手
    protected Dictionary<Finger.FingerType, FingerData>[] m_FingerDatas = new Dictionary<Finger.FingerType, FingerData>[2];

    protected Dictionary<Finger.FingerType, FingerData>[,] m_FingerDatasBuffer = new Dictionary<Finger.FingerType, FingerData>[2, BUFFER_MAX];//保存5组数据放在buffer中
    protected int m_CurBufIndex = 0;

    private readonly JointData m_DefaultJointData = new JointData(Vector.Zero, Vector.Zero);
    private readonly FingerData m_DefaultFingerData = new FingerData(Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero, Vector.Zero);


    private Finger.FingerType[] finger_Type = new Finger.FingerType[5] { Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_RING , Finger.FingerType.TYPE_PINKY };
    // private JointData[] joint_Type = new JointData[5] { FingerData.meta0, FingerData.prox0, FingerData.inter0, FingerData.dist0, FingerData.dist1 };
    //private JointData[] joint_Type = new JointData[5] { meta0, prox0, inter0, dist0, dist1 };

    public int frame_index;
    public GameStatus gameStatus;
	public GameObject[] objects;

    public int frameNum=100;

    public StandardActionLibrary stdAction;

    public string defaultPath = "/Datas/test02.json";

    void AddDefaultFingerData()
    {
        DicAddDefaultData(m_FingerDatas[0]);
        DicAddDefaultData(m_FingerDatas[1]);
        for(int i=0; i<2; i++)
        {
            for(int j=0; j<BUFFER_MAX; j++)
            {
                DicAddDefaultData(m_FingerDatasBuffer[i, j]); 
            }
        }
    }

    void DicAddDefaultData(Dictionary<Finger.FingerType, FingerData> dic)
    {
        dic.Add(Finger.FingerType.TYPE_THUMB, m_DefaultFingerData);
        dic.Add(Finger.FingerType.TYPE_INDEX, m_DefaultFingerData);
        dic.Add(Finger.FingerType.TYPE_MIDDLE, m_DefaultFingerData);
        dic.Add(Finger.FingerType.TYPE_RING, m_DefaultFingerData);
        dic.Add(Finger.FingerType.TYPE_PINKY, m_DefaultFingerData);    
    }

    void DicUseDefaultData(Dictionary<Finger.FingerType, FingerData> dic)
    {
        dic[Finger.FingerType.TYPE_THUMB] = m_DefaultFingerData;
        dic[Finger.FingerType.TYPE_INDEX] = m_DefaultFingerData;
        dic[Finger.FingerType.TYPE_MIDDLE] = m_DefaultFingerData;
        dic[Finger.FingerType.TYPE_RING] = m_DefaultFingerData;
        dic[Finger.FingerType.TYPE_PINKY] = m_DefaultFingerData;

    }

    //设置左右手初始位置与旋转方向
    void AddDefaultOverallHandData()
    {
        Vector left_hand_pos = new Vector(0, 0, 0);
        Vector left_hand_rot = new Vector(0, 0, 0);
        Vector right_hand_pos = new Vector(0, 0, 0);
        Vector right_hand_rot = new Vector(0, 0, 0);
        m_hand_position[0].Set(left_hand_pos, left_hand_rot);
        m_hand_position[1].Set(right_hand_pos, right_hand_rot);
    }

	// Use this for initialization
	void Start () {
        frame_index = 0;
        startSaveFrame = 0;
        gameStatus = new GameStatus();
        stdAction = new StandardActionLibrary();
        gameStatus.frame_data = new refencenes0[frameNum];  // json里存500个帧的数据

        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    void Awake()
    {
        delta = 1.0 / 24.0;
        m_FingerDatas[0] = new Dictionary<Finger.FingerType, FingerData>();
        m_FingerDatas[1] = new Dictionary<Finger.FingerType, FingerData>();

        for(int i=0; i<2; i++)
        {
            for(int j=0; j<BUFFER_MAX; j++)
            {
                m_FingerDatasBuffer[i, j] = new Dictionary<Finger.FingerType, FingerData>();
            }
        }
        DicAddDefaultData(m_FingerDatas[0]);
        DicAddDefaultData(m_FingerDatas[1]);
    }
    
    // Update is called once per frame
    void Update () {
        
        Frame frame = provider.CurrentFrame;
        SaveOverallHandData(frame);
        SaveFingerFrame(frame);

        if (Input.GetKeyDown(KeyCode.S))
        {
            startSaveFrame = 1;
        }
        if (frame_index == frameNum)
        {
            SaveJson();
            frame_index++;
            startSaveFrame = 0;
        }

		if (Input.GetKeyDown(KeyCode.L)){
			ReadJson ();
		}

        if (startSaveFrame == 1)
        {
            if (first_get == 0)
            {
                old_Time = DateTime.Now;
                Debug.Log("frame_index:::" + frame_index);
                setHandDatas(frame_index);
                frame_index = frame_index + 1;
                first_get = 1;
            }
            now_Time = DateTime.Now;
            time_Span = now_Time - old_Time;
            total_Seconds = time_Span.TotalSeconds;
            if (total_Seconds >= delta) {
                first_get = 0;
            }
        }
        
    }

    public void SaveOverallHandData(Frame frame)
    {
        Quaternion rot;
        Hand leftHand=null,rightHand=null;

        foreach(Hand hand in frame.Hands)
        {
            if (hand.IsLeft) leftHand = hand;
            if (leftHand != null) break;
        }
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight) rightHand = hand;
            if (leftHand != null) break;
        }

        if (leftHand != null)
        {
            m_hand_position[0].hand_position = leftHand.WristPosition;
            rot = leftHand.Basis.CalculateRotation();
            Vector tmp1 = new Vector(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
            m_hand_position[0].hand_rotation = tmp1;
        }
        if (rightHand != null)
        {
            m_hand_position[1].hand_position = rightHand.WristPosition;
            rot = rightHand.Basis.CalculateRotation();
            Vector tmp2 = new Vector(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
            m_hand_position[1].hand_rotation = tmp2;
        }
    }

    public void SaveFingerFrame(Frame frame)
    {
        foreach(Hand hand in frame.Hands)
        {
            SaveFingerDataWithHandIndex(hand);
        }
    }

    void SaveFingerDataWithHandIndex(Hand hand)
    {
        bool isLeft = hand.IsLeft;
        int handIndex = isLeft ? 0 : 1;
        JointData meta0;
        JointData prox0;
        JointData inter0;
        JointData dist0;
        JointData dist1;

        foreach (Finger finger in hand.Fingers)
        {
            Finger.FingerType fingerType = finger.Type;
            
            if(fingerType == Finger.FingerType.TYPE_THUMB)
            {
                meta0 = new JointData(Vector.Zero, Vector.Zero);
            }
            else
            {
                meta0 = GetJointData(finger, Bone.BoneType.TYPE_METACARPAL, 0);
            }
            prox0 = GetJointData(finger, Bone.BoneType.TYPE_PROXIMAL, 0);
            inter0 = GetJointData(finger, Bone.BoneType.TYPE_INTERMEDIATE, 0);
            dist0 = GetJointData(finger, Bone.BoneType.TYPE_DISTAL, 0);
            dist1 = GetJointData(finger, Bone.BoneType.TYPE_DISTAL, 1);

            FingerData finger_data = new FingerData(meta0, prox0, inter0, dist0, dist1);

            SaveFingerData(handIndex, fingerType, finger_data);
        }

    }

    JointData GetJointData(Finger finger, Bone.BoneType bonetype, int pn)
    {
        Quaternion rot;
        Vector position;
        Vector rotation;

        if (pn == 0)
        {
            position = finger.Bone(bonetype).PrevJoint;
        }
        else
        {
            position = finger.Bone(bonetype).NextJoint;
        }
        rot = finger.Bone(bonetype).Basis.CalculateRotation();
        Vector tmp = new Vector(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
        rotation = tmp;

        JointData joint_data = new JointData(position, rotation);
        return joint_data;
    }

    void SaveFingerData(int handIndex, Finger.FingerType fingerType, FingerData fingerData)
    {
        if (m_FingerDatas[handIndex].ContainsKey(fingerType))
        {
            m_FingerDatas[handIndex][fingerType] = fingerData;
        }
        else
        {
            m_FingerDatas[handIndex].Add(fingerType, fingerData);
        }
    }


	// ————————————————————————————————————————————————————————————————————————————————————————————————————
	public void setHandDatas(int i) {
		//gameStatus = new GameStatus ();
		//gameStatus.frame_data = new refencenes0[10];
		
		gameStatus.frame_data [i] = new refencenes0 ();
		gameStatus.frame_data [i].frame_index = i+1;
		gameStatus.frame_data[i].m_hand_position = new refencenes1[2];
		setHandPosition (gameStatus.frame_data[i].m_hand_position);
		gameStatus.frame_data[i].m_FingerDatas = new refencenes2[2];
		setFingerDatas (gameStatus.frame_data[i].m_FingerDatas);
		

//		gameStatus.m_hand_position = new refencenes1[2];
//		setHandPosition (gameStatus.m_hand_position);
//
//		gameStatus.m_FingerDatas = new refencenes2[2];
//		//		gameStatus.m_FingerDatas [0].finger_type [0] = new vec[1];
//		setFingerDatas (gameStatus.m_FingerDatas);
	}
	void setHandPosition(refencenes1[] hand_position) {
        for (int i = 0; i < 2; i++)
        {
            hand_position[i] = new refencenes1();
            hand_position[i].hand_type = i;

            hand_position[i].hand_position = new vec[1];
            hand_position[i].hand_position[0] = new vec();
            //Debug.Log("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            //Debug.Log("hand position: " + m_hand_position[0].hand_position[0].ToString());
            hand_position[i].hand_position[0].x = m_hand_position[i].hand_position[0].ToString();
            hand_position[i].hand_position[0].y = m_hand_position[i].hand_position[1].ToString();
            hand_position[i].hand_position[0].z = m_hand_position[i].hand_position[2].ToString();

            hand_position[i].hand_rotation = new vec[1];
            hand_position[i].hand_rotation[0] = new vec();
            hand_position[i].hand_rotation[0].x = m_hand_position[i].hand_rotation[0].ToString();
            hand_position[i].hand_rotation[0].y = m_hand_position[i].hand_rotation[1].ToString();
            hand_position[i].hand_rotation[0].z = m_hand_position[i].hand_rotation[2].ToString();
        }
    }

	void setFingerDatas(refencenes2[] finger_datas) {
		for (int i = 0; i < 2; i++) {
			finger_datas [i] = new refencenes2 ();
			finger_datas [i].hand_type = i;

			finger_datas [i].TYPE_THUMB = new finger[1];
			setJointDatas (finger_datas [i].TYPE_THUMB, i, 0);
			finger_datas [i].TYPE_INDEX = new finger[1];
			setJointDatas (finger_datas [i].TYPE_INDEX, i, 1);
			finger_datas [i].TYPE_MIDDLE = new finger[1];
			setJointDatas (finger_datas [i].TYPE_MIDDLE, i, 2);
			finger_datas [i].TYPE_RING = new finger[1];
			setJointDatas (finger_datas [i].TYPE_RING, i, 3);
			finger_datas [i].TYPE_PINKY = new finger[1];
			setJointDatas (finger_datas [i].TYPE_PINKY, i, 4);
		}
	}

	void setJointDatas(finger[] joint_datas, int hand_type, int finger_type) {
		joint_datas [0] = new finger ();
		joint_datas[0].finger_type = finger_type;

		joint_datas [0].meta0 = new joint[1];
		setVecDatas (joint_datas [0].meta0, hand_type, finger_type, 0);
		joint_datas [0].prox0 = new joint[1];
		setVecDatas (joint_datas [0].prox0, hand_type, finger_type, 1);
		joint_datas [0].inter0 = new joint[1];
		setVecDatas (joint_datas [0].inter0, hand_type, finger_type, 2);
		joint_datas [0].dist0 = new joint[1];
		setVecDatas (joint_datas [0].dist0, hand_type, finger_type, 3);
		joint_datas [0].dist1 = new joint[1];
		setVecDatas (joint_datas [0].dist1, hand_type, finger_type, 4);

	}

	void setVecDatas(joint[] vec_datas, int hand_type, int finger_type, int joint_type){
		vec_datas[0] = new joint();
		vec_datas[0].joint_type = joint_type;
		//meta0
        if(joint_type == 0)
        {
            //pos
            vec_datas[0].joint_position = new vec[1];
            vec_datas[0].joint_position[0] = new vec();
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            Debug.Log("hand_type: " + hand_type + "\n" + "finger_type: " + finger_type + "\n joint_type: " + joint_type);

            Debug.Log("joint position: " + m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_position.ToString());
            Debug.Log("joint position: " + m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_position[0].ToString());
            vec_datas[0].joint_position[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_position[0].ToString();
            //vec_datas[0].joint_position[0].x = "1";
            vec_datas[0].joint_position[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_position[1].ToString();
            vec_datas[0].joint_position[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_position[2].ToString();
            //rot
            vec_datas[0].joint_rotation = new vec[1];
            vec_datas[0].joint_rotation[0] = new vec();
            vec_datas[0].joint_rotation[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_rotation[0].ToString();
            vec_datas[0].joint_rotation[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_rotation[1].ToString();
            vec_datas[0].joint_rotation[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].meta0.joint_rotation[2].ToString();
        }
        //prox0
        else if(joint_type == 1)
        {
            //pos
            vec_datas[0].joint_position = new vec[1];
            vec_datas[0].joint_position[0] = new vec();
            vec_datas[0].joint_position[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].prox0.joint_position[0].ToString();
            vec_datas[0].joint_position[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].prox0.joint_position[1].ToString();
            vec_datas[0].joint_position[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].prox0.joint_position[2].ToString();
            //rot
            vec_datas[0].joint_rotation = new vec[1];
            vec_datas[0].joint_rotation[0] = new vec();
            vec_datas[0].joint_rotation[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].prox0.joint_rotation[0].ToString();
            vec_datas[0].joint_rotation[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].prox0.joint_rotation[1].ToString();
            vec_datas[0].joint_rotation[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].prox0.joint_rotation[2].ToString();

            ////pos
            //vec_datas[0].joint_position = new vec[1];
            //vec_datas[0].joint_position[0] = new vec();
            //vec_datas[0].joint_position[0].x = "10";
            //vec_datas[0].joint_position[0].y = "1";
            //vec_datas[0].joint_position[0].z = "1";
            ////rot
            //vec_datas[0].joint_rotation = new vec[1];
            //vec_datas[0].joint_rotation[0] = new vec();
            //vec_datas[0].joint_rotation[0].x = "1";
            //vec_datas[0].joint_rotation[0].y = "1";
            //vec_datas[0].joint_rotation[0].z = "1";
        }
        //inter0
        else if(joint_type == 2)
        {
            //pos
            vec_datas[0].joint_position = new vec[1];
            vec_datas[0].joint_position[0] = new vec();
            vec_datas[0].joint_position[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].inter0.joint_position[0].ToString();
            vec_datas[0].joint_position[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].inter0.joint_position[1].ToString();
            vec_datas[0].joint_position[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].inter0.joint_position[2].ToString();
            //rot
            vec_datas[0].joint_rotation = new vec[1];
            vec_datas[0].joint_rotation[0] = new vec();
            vec_datas[0].joint_rotation[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].inter0.joint_rotation[0].ToString();
            vec_datas[0].joint_rotation[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].inter0.joint_rotation[1].ToString();
            vec_datas[0].joint_rotation[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].inter0.joint_rotation[2].ToString();
        }
        //dist0
        else if(joint_type == 3)
        {
            //pos
            vec_datas[0].joint_position = new vec[1];
            vec_datas[0].joint_position[0] = new vec();
            vec_datas[0].joint_position[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].dist0.joint_position[0].ToString();
            vec_datas[0].joint_position[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].dist0.joint_position[1].ToString();
            vec_datas[0].joint_position[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].dist0.joint_position[2].ToString();
            //rot
            vec_datas[0].joint_rotation = new vec[1];
            vec_datas[0].joint_rotation[0] = new vec();
            vec_datas[0].joint_rotation[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].dist0.joint_rotation[0].ToString();
            vec_datas[0].joint_rotation[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].dist0.joint_rotation[1].ToString();
            vec_datas[0].joint_rotation[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].dist0.joint_rotation[2].ToString();
        }
        //dist1
        else if(joint_type == 4)
        {
            //pos
            vec_datas[0].joint_position = new vec[1];
            vec_datas[0].joint_position[0] = new vec();
            vec_datas[0].joint_position[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].dist1.joint_position[0].ToString();
            vec_datas[0].joint_position[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].dist1.joint_position[1].ToString();
            vec_datas[0].joint_position[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].dist1.joint_position[2].ToString();
            //rot
            vec_datas[0].joint_rotation = new vec[1];
            vec_datas[0].joint_rotation[0] = new vec();
            vec_datas[0].joint_rotation[0].x = m_FingerDatas[hand_type][finger_Type[finger_type]].dist1.joint_rotation[0].ToString();
            vec_datas[0].joint_rotation[0].y = m_FingerDatas[hand_type][finger_Type[finger_type]].dist1.joint_rotation[1].ToString();
            vec_datas[0].joint_rotation[0].z = m_FingerDatas[hand_type][finger_Type[finger_type]].dist1.joint_rotation[2].ToString();
        }
       
    }

	public void SaveJson ()
	{
		string json = JsonUtility.ToJson (gameStatus);
		DateTime saveTime = DateTime.Now;
        string action_id = stdAction.getActionId();
		string savePath = Application.dataPath + "/StandardActionLibrary/" + action_id + ".json";
        Debug.Log("savepath" + savePath);
        File.WriteAllText (savePath, json, Encoding.UTF8);
		Debug.Log ("save:::" + savePath);
	}

	public void ReadJson ()
	{
		GameStatus status = LoadJson.LoadJsonFromFile();
		for (int a = 0; a < status.frame_data.Length; a++) { 
			for (int i = 0; i < 2; i++) {
				Vector temp1 = new Vector ( float.Parse(status.frame_data[a].m_hand_position[i].hand_position[0].x), float.Parse(status.frame_data[a].m_hand_position[i].hand_position[0].y), float.Parse(status.frame_data[a].m_hand_position[i].hand_position[0].z) );
				m_hand_position[i].hand_position = temp1;

				Vector temp2 = new Vector ( float.Parse(status.frame_data[a].m_hand_position[i].hand_rotation[0].x), float.Parse(status.frame_data[a].m_hand_position[i].hand_rotation[0].y), float.Parse(status.frame_data[a].m_hand_position[i].hand_rotation[0].z) );
				m_hand_position[i].hand_rotation = temp2;
			}

            for (int i = 0; i < 2; i++) {
                readJointDataFromJson(status.frame_data[a].m_FingerDatas[i].TYPE_THUMB, i, 0);
                readJointDataFromJson(status.frame_data[a].m_FingerDatas[i].TYPE_INDEX, i, 1);
                readJointDataFromJson(status.frame_data[a].m_FingerDatas[i].TYPE_MIDDLE, i, 2);
                readJointDataFromJson(status.frame_data[a].m_FingerDatas[i].TYPE_RING, i, 3);
                readJointDataFromJson(status.frame_data[a].m_FingerDatas[i].TYPE_PINKY, i, 4);
            }
        }

    }

    public void ReadJsonByFrame(int frameNo,GameStatus status)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector temp1 = new Vector(float.Parse(status.frame_data[frameNo].m_hand_position[i].hand_position[0].x), float.Parse(status.frame_data[frameNo].m_hand_position[i].hand_position[0].y), float.Parse(status.frame_data[frameNo].m_hand_position[i].hand_position[0].z));
            m_hand_position[i].hand_position = temp1;

            Vector temp2 = new Vector(float.Parse(status.frame_data[frameNo].m_hand_position[i].hand_rotation[0].x), float.Parse(status.frame_data[frameNo].m_hand_position[i].hand_rotation[0].y), float.Parse(status.frame_data[frameNo].m_hand_position[i].hand_rotation[0].z));
            m_hand_position[i].hand_rotation = temp2;
            //Debug.Log("rotation:"+temp2);
        }

        for (int i = 0; i < 2; i++)
        {
            readJointDataFromJson(status.frame_data[frameNo].m_FingerDatas[i].TYPE_THUMB, i, 0);
            readJointDataFromJson(status.frame_data[frameNo].m_FingerDatas[i].TYPE_INDEX, i, 1);
            readJointDataFromJson(status.frame_data[frameNo].m_FingerDatas[i].TYPE_MIDDLE, i, 2);
            readJointDataFromJson(status.frame_data[frameNo].m_FingerDatas[i].TYPE_RING, i, 3);
            readJointDataFromJson(status.frame_data[frameNo].m_FingerDatas[i].TYPE_PINKY, i, 4);
        }
    }

    public void readJointDataFromJson(finger[] a, int hand_type, int finger_type)
    {
        //meta0
        Vector temp111 = new Vector(float.Parse(a[0].meta0[0].joint_position[0].x), float.Parse(a[0].meta0[0].joint_position[0].y), float.Parse(a[0].meta0[0].joint_position[0].z));
        Vector temp112 = new Vector(float.Parse(a[0].meta0[0].joint_rotation[0].x), float.Parse(a[0].meta0[0].joint_rotation[0].y), float.Parse(a[0].meta0[0].joint_rotation[0].z));
        JointData joint_m = new JointData(temp111, temp112);
        // prox0
        Vector temp121 = new Vector(float.Parse(a[0].prox0[0].joint_position[0].x), float.Parse(a[0].prox0[0].joint_position[0].y), float.Parse(a[0].prox0[0].joint_position[0].z));
        Vector temp122 = new Vector(float.Parse(a[0].prox0[0].joint_rotation[0].x), float.Parse(a[0].prox0[0].joint_rotation[0].y), float.Parse(a[0].prox0[0].joint_rotation[0].z));
        JointData joint_p = new JointData(temp121, temp122);
        // inter0
        Vector temp131 = new Vector(float.Parse(a[0].inter0[0].joint_position[0].x), float.Parse(a[0].inter0[0].joint_position[0].y), float.Parse(a[0].inter0[0].joint_position[0].z));
        Vector temp132 = new Vector(float.Parse(a[0].inter0[0].joint_rotation[0].x), float.Parse(a[0].inter0[0].joint_rotation[0].y), float.Parse(a[0].inter0[0].joint_rotation[0].z));
        JointData joint_i = new JointData(temp131, temp132);
        // dist0
        Vector temp141 = new Vector(float.Parse(a[0].dist0[0].joint_position[0].x), float.Parse(a[0].dist0[0].joint_position[0].y), float.Parse(a[0].dist0[0].joint_position[0].z));
        Vector temp142 = new Vector(float.Parse(a[0].dist0[0].joint_rotation[0].x), float.Parse(a[0].dist0[0].joint_rotation[0].y), float.Parse(a[0].dist0[0].joint_rotation[0].z));
        JointData joint_d0 = new JointData(temp141, temp142);
        // dist1
        Vector temp151 = new Vector(float.Parse(a[0].dist1[0].joint_position[0].x), float.Parse(a[0].dist1[0].joint_position[0].y), float.Parse(a[0].dist1[0].joint_position[0].z));
        Vector temp152 = new Vector(float.Parse(a[0].dist1[0].joint_rotation[0].x), float.Parse(a[0].dist1[0].joint_rotation[0].y), float.Parse(a[0].dist1[0].joint_rotation[0].z));
        JointData joint_d1 = new JointData(temp151, temp152);
        FingerData finger = new FingerData(joint_m, joint_p, joint_i, joint_d0, joint_d1);
        m_FingerDatas[hand_type][finger_Type[finger_type]] = finger;

    }

}
