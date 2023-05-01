﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIMuseumVR.SceneManagement.NotUsed
{
    /// <summary> Serializable version of UnityEngine.Vector3. </summary>
    [System.Serializable]
    public struct SVector3
    {
        public float x;
        public float y;
        public float z;

        public SVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
            => string.Format("[{0}, {1}, {2}]", x, y, z);


        public static implicit operator Vector3(SVector3 s)
            => new Vector3(s.x, s.y, s.z);

        public static implicit operator SVector3(Vector3 v)
            => new SVector3(v.x, v.y, v.z);


        public static SVector3 operator +(SVector3 a, SVector3 b)
            => new SVector3(a.x + b.x, a.y + b.y, a.z + b.z);

        public static SVector3 operator -(SVector3 a, SVector3 b)
            => new SVector3(a.x - b.x, a.y - b.y, a.z - b.z);

        public static SVector3 operator -(SVector3 a)
            => new SVector3(-a.x, -a.y, -a.z);

        public static SVector3 operator *(SVector3 a, float m)
            => new SVector3(a.x * m, a.y * m, a.z * m);

        public static SVector3 operator *(float m, SVector3 a)
            => new SVector3(a.x * m, a.y * m, a.z * m);

        public static SVector3 operator /(SVector3 a, float d)
            => new SVector3(a.x / d, a.y / d, a.z / d);
    }

    /// <summary> Serializable version of UnityEngine.Quaternion. </summary>
    [System.Serializable]
    public struct SQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override string ToString()
            => $"[{x}, {y}, {z}, {w}]";

        public static implicit operator Quaternion(SQuaternion s)
            => new Quaternion(s.x, s.y, s.z, s.w);

        public static implicit operator SQuaternion(Quaternion q)
            => new SQuaternion(q.x, q.y, q.z, q.w);
    }
}