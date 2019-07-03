using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class GameStatus
{
	public refencenes0[] frame_data;

}

[Serializable]
public class refencenes0
{
	public refencenes0()
	{
		frame_index = -1;
	}
	public int frame_index;
	public refencenes1[] m_hand_position;
	public refencenes2[] m_FingerDatas;
}


[Serializable]
public class refencenes1
{
	public refencenes1()
	{
		hand_type = -1;
	}
	public int hand_type;
	public vec[] hand_position;
	public vec[] hand_rotation;
}


[Serializable]
public class vec
{
	public vec()
	{
		x = "-1";
		y = "-1";
		z = "-1";
	}
	public string x;
	public string y;
	public string z;
}



[Serializable]
public class refencenes2
{
	public refencenes2()
	{
		hand_type = -1;
	}
	public int hand_type;
	public finger[] TYPE_THUMB;
	public finger[] TYPE_INDEX;
	public finger[] TYPE_MIDDLE;
	public finger[] TYPE_RING;
	public finger[] TYPE_PINKY;

}

[Serializable]
public class finger
{
	public finger()
	{
		finger_type = -1;
	}
	public int finger_type;
	public joint[] meta0;
	public joint[] prox0;
	public joint[] inter0;
	public joint[] dist0;
	public joint[] dist1;
}

[Serializable]
public class joint
{
	public joint()
	{
		joint_type = -1;
	}
	public int joint_type;
	public vec[] joint_position;
	public vec[] joint_rotation;
}
