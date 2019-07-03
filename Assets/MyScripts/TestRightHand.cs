using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class TestRightHand : MonoBehaviour {
    LeapProvider provider;
    List<Hand> rightHands;

    // Use this for initialization
    void Start () {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        rightHands = new List<Hand>();
    }
	
	// Update is called once per frame
	void Update () {
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                rightHands.Add(hand);
            }
            if (rightHands.Count != 0) break;
        }

        if (rightHands.Count != 0)
        {
            Hand hand = rightHands[0];

            transform.position = hand.WristPosition.ToVector3();
            transform.rotation = hand.Basis.CalculateRotation();

            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.name == "thumb_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_THUMB)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.transform.Rotate(new Vector3(0, -90, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).transform.Rotate(new Vector3(0, -90, 0));
                        }
                    }
                }
                if (t.name == "index_finger_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_INDEX)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
                if (t.name == "middle_finger_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_MIDDLE)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
                if (t.name == "ring_finger_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_RING)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
                if (t.name == "pinky_1")
                {
                    foreach (Finger finger in hand.Fingers)
                    {
                        if (finger.Type == Finger.FingerType.TYPE_PINKY)
                        {
                            t.transform.rotation = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Basis.CalculateRotation();
                            t.transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Basis.CalculateRotation();
                            t.GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                            t.GetChild(0).GetChild(0).rotation = finger.Bone(Bone.BoneType.TYPE_DISTAL).Basis.CalculateRotation();
                            t.GetChild(0).GetChild(0).transform.Rotate(new Vector3(-90, 0, 0));
                        }
                    }
                }
            }
        }
        else
        {
            transform.position = new Vector3(10, 10, -20);
        }
        rightHands.Clear();
    }
}
