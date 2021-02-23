using System;
using System.Collections.Generic;
using UnityEngine;

namespace FieldOfView
{
    [Serializable]
    public struct Data
    {
        public Vector3 origin;
        public List<Vector3> targets;
        public float maxDistance;
    }
}