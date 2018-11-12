using System;
using System.Collections;
using System.Collections.Generic;

using nuitrack;
using Vector3 = UnityEngine.Vector3;

[Serializable]
public struct SerializableJoint {

    public JointType Type;
    public float Confidence; 
    public Vector3 Real;
    public Vector3 Proj;
    public Orientation Orient;

    public static implicit operator Joint(SerializableJoint j) {
        return new Joint {
            Type = j.Type,
            Confidence = j.Confidence,
            Real = j.Real.ToNuitrackVector3(),
            Proj = j.Proj.ToNuitrackVector3(),
            Orient = j.Orient
        };
    }

    public static implicit operator SerializableJoint(Joint j) {
        return new SerializableJoint{
            Type = j.Type,
            Confidence = j.Confidence,
            Real = j.Real.ToUnityVector3(),
            Proj = j.Proj.ToUnityVector3(),
            Orient = j.Orient
        };
    }
}
