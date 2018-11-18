using System;
using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;

[Serializable]
public class RiggedModelJoint {

    public ISkeletonProvider SkeletonProvider { get; set; }

    public Transform Bone;
    public JointType JointType;

    [HideInInspector] public Quaternion BaseRotOffset;

    void FixedUpdate() {
        if (SkeletonProvider?.CurSkeleton != null) {
            //UpdateAngle(SkeletonProvider.CurSkeleton);
        }
    }

    public void Colorize(Color c) {

    }

    public void UpdateAngle(Skeleton s) {
        Joint joint = s.GetJoint(JointType);
        Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * BaseRotOffset;
        Bone.rotation = jointOrientation;
    }
}
