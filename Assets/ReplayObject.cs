using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyReplay
{
    //添加对象将会被记录
    public class ReplayObject : MonoBehaviour
    {
        public ReplayEntityType entityType = ReplayEntityType.None;
        public int entityIndex = 0;

    }
}
