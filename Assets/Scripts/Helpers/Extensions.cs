using System;
using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public static class Extensions {

    public static Vector3 ToUnityVector3(this nuitrack.Vector3 v) {
        return new Vector3(v.X, v.Y, v.Z);
    }

    public static nuitrack.Vector3 ToNuitrackVector3(this Vector3 v) {
        return new nuitrack.Vector3(v.x, v.y, v.z);
    }

    public static float Squared(this float f) {
        return Mathf.Pow(f, 2);
    }
}
