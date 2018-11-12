using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using nuitrack;
using Joint = nuitrack.Joint;

[Serializable]
public class SerializableSkeleton {

    public int ID;
    public SerializableJoint[] Joints;

    public SerializableSkeleton() {}

    public SerializableSkeleton(int id, Joint[] joints) {
        ID = id;
        Joints = joints.Select(j => (SerializableJoint) j).ToArray();
    }

    public Joint this[JointType type] {
        get { return Joints.Single(j => j.Type == type); }
    }

    public static implicit operator Skeleton(SerializableSkeleton s) {
        if (s == null)
            return null;

        return new Skeleton(s.ID, s.Joints.Select(j => (Joint) j).ToArray());
    }

    public static implicit operator SerializableSkeleton(Skeleton s) {
        if (s == null)
            return null;

        return new SerializableSkeleton(s.ID, s.Joints);
    }
}
