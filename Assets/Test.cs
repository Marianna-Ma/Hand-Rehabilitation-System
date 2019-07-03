using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public enum aaa {
        none=0,a,b,c,d
    }

    private void Update()
    {
        int i = 1 << ((int)aaa.none);
        Debug.Log(i.ToString());
    }
}
