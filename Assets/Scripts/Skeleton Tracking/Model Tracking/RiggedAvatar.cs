using System;
using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;
using Vector3 = UnityEngine.Vector3;

public class RiggedAvatar : MonoBehaviour, IAvatar, ISkeletonProvider {

    [Header("Rigged Model")] public RiggedModelJoint[] ModelJoints;

    public ISkeletonProvider SkeletonProvider { get; protected set; }
    public SerializableSkeleton CurSkeleton => SkeletonProvider.CurSkeleton;

    void Start() {
        SkeletonProvider = CurrentUserTracker.Instance;
    }

    void FixedUpdate() {
        if (SkeletonProvider?.CurSkeleton != null) {
            PositionSkeleton(SkeletonProvider.CurSkeleton);
            CalculateRotation(SkeletonProvider.CurSkeleton);
        }
    }

    public void SwapSkeletonProvider(ISkeletonProvider newProvider) {
        SkeletonProvider = newProvider;
    }

    private void PositionSkeleton(Skeleton s) {
        Vector3 torsoPos = Quaternion.Euler(Vector3.zero) * (0.001f * s.GetJoint(JointType.Torso).ToVector3());
        transform.position = torsoPos;
    }

    private void CalculateRotation(Skeleton s) {
        foreach (RiggedModelJoint riggedModelJoint in ModelJoints) {
            Joint joint = s.GetJoint(riggedModelJoint.JointType);
            Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * riggedModelJoint.BaseRotOffset;
            riggedModelJoint.Bone.rotation = jointOrientation;
        }
    }
}
