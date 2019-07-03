using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;

namespace LeapEvaluate
{
    

    public class evaluate : MonoBehaviour
    {
        //声明
        private ArrayList TAM;
        private float angle, temp1, temp2;
        private float [] angle1=new float [5];
        LeapProvider provider;
        int tamIndex;
        float m_timer = 0;
        List<Frame> framelist;
        Frame[] flist = new Frame[5];
        int count = 0;

        public double[] temp = new double[5];

        // Use this for initialization
        void Start()
        {
            provider = FindObjectOfType<LeapProvider>() as LeapProvider;
            TAM = new ArrayList();
            TAM.Add(1);
            TAM.Add(1);
            TAM.Add(1);
            TAM.Add(1);
            TAM.Add(1);
            tamIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            count++;
            if (count >= 200)
            {
                Evaluate();
                Pout();
                count = 0;
            }
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(5);
            Evaluate();
            Pout();
        }

        void addFrame(Frame f)
        {
            framelist.Add(f);
        }

        public void Evaluate()
        {
            Frame frame = provider.CurrentFrame;
            tamIndex = 0;
            //foreach (Frame f in framelist)
            //{
                //Hand hand = frame.Hands[0];
                foreach (Hand hand in frame.Hands)
                {
                    tamIndex = 0;
                    foreach (Finger finger in hand.Fingers)
                    {

                        if (finger.Type == Finger.FingerType.TYPE_THUMB)
                        {
                            angle = 0f;
                            for (int i = 0; i < 2; i++)
                            {
                                temp1 = finger.Bone(ToBone(i)).Basis.CalculateRotation().x * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().x
                                    + finger.Bone(ToBone(i)).Basis.CalculateRotation().y * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().y
                                    + finger.Bone(ToBone(i)).Basis.CalculateRotation().z * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().z;
                                temp2 = Mathf.Sqrt(finger.Bone(ToBone(i)).Basis.CalculateRotation().x * finger.Bone(ToBone(i)).Basis.CalculateRotation().x +
                                    finger.Bone(ToBone(i)).Basis.CalculateRotation().y * finger.Bone(ToBone(i)).Basis.CalculateRotation().y +
                                    finger.Bone(ToBone(i)).Basis.CalculateRotation().z * finger.Bone(ToBone(i)).Basis.CalculateRotation().z) *
                                    Mathf.Sqrt(finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().x * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().x +
                                    finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().y * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().y +
                                    finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().z * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().z);
                                temp1 = temp1 / temp2;
                                angle += Mathf.Acos(temp1);
                            }
                            TAM[tamIndex] = Mathf.Abs(angle / 1.919f);
                        }
                        else
                        {
                            angle = 0f;
                            for (int i = 1; i < 3; i++)
                            {
                                temp1 = finger.Bone(ToBone(i)).Basis.CalculateRotation().x * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().x
                                    + finger.Bone(ToBone(i)).Basis.CalculateRotation().y * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().y
                                    + finger.Bone(ToBone(i)).Basis.CalculateRotation().z * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().z;
                                temp2 = Mathf.Sqrt(finger.Bone(ToBone(i)).Basis.CalculateRotation().x * finger.Bone(ToBone(i)).Basis.CalculateRotation().x +
                                    finger.Bone(ToBone(i)).Basis.CalculateRotation().y * finger.Bone(ToBone(i)).Basis.CalculateRotation().y +
                                    finger.Bone(ToBone(i)).Basis.CalculateRotation().z * finger.Bone(ToBone(i)).Basis.CalculateRotation().z) *
                                    Mathf.Sqrt(finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().x * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().x +
                                    finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().y * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().y +
                                    finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().z * finger.Bone(ToBone(i + 1)).Basis.CalculateRotation().z);
                                temp1 = temp1 / temp2;
                                //angle = Mathf.Acos(temp1);
                                //Debug.Log(angle);
                                angle += Mathf.Acos(temp1);
                            }
                            TAM[tamIndex] = Mathf.Abs(angle / 3.66f);
                        }
                        tamIndex++;
                    }  
                }
           // }
        }
        public Bone.BoneType ToBone(int i)
        {
            if (i == 1) return Bone.BoneType.TYPE_PROXIMAL;
            else if (i == 2) return Bone.BoneType.TYPE_INTERMEDIATE;
            else if (i == 3) return Bone.BoneType.TYPE_DISTAL;
            else if (i == 0) return Bone.BoneType.TYPE_METACARPAL;
            else return Bone.BoneType.TYPE_INVALID;
        }

        public void Pout()
        {
            //Debug.Log(TAM[0]);
            //for (int i = 0; i < 5; i++)
            //{
            //    Debug.Log(TAM[i]);
            //}
            temp = (double[])TAM.ToArray(typeof(double));
            if (temp[0] < 0.3)
                Debug.Log("拇指评价值：" + "可\n");
            else if(temp[0]<0.4)
                Debug.Log("拇指评价值：" + "良\n");
            else if(temp[0]>0.5)
                Debug.Log("拇指评价值：" + "优\n");
            for (int i = 1; i < 5; i++)
            {
                if (temp[i] < 0.2) Debug.Log(ToFinger(i) + "评价值：" + "可\n");
                else if (temp[i] > 0.9) Debug.Log(ToFinger(i) + "评价值：" + "良\n");
                else if (temp[i] > 0.6 & temp[i] < 0.8) Debug.Log(ToFinger(i) + "评价值：" + "优\n");
            }
        }
        
        public string ToFinger(int i)
        {
            if (i == 1) return "食指";
            else if (i == 2) return "中指";
            else if (i == 3) return "无名指";
            else return "小拇指";
        }
    }
}

