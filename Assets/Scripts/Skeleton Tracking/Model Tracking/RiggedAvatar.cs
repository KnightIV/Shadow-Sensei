﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;
using Vector3 = UnityEngine.Vector3;

public class RiggedAvatar : MonoBehaviour, IAvatar {

    [Header("Rigged Model")] public RiggedModelJoint[] ModelJoints;
    public bool Bound;
    public Vector3 BoundPos;

    public ISkeletonProvider SkeletonProvider { get; protected set; }
    public SerializableSkeleton CurSkeleton => SkeletonProvider.CurSkeleton;

    void Start() {
        SkeletonProvider = CurrentUserTracker.Instance;

        foreach (RiggedModelJoint modelJoint in ModelJoints) {
            modelJoint.BaseRotOffset = modelJoint.Bone.rotation;
        }
    }

    void FixedUpdate() {
        if (SkeletonProvider?.CurSkeleton != null) {
            PositionSkeleton(SkeletonProvider.CurSkeleton);
            RotateBones(SkeletonProvider.CurSkeleton);
        }
    }

    public void SwapSkeletonProvider(ISkeletonProvider newProvider) {
        SkeletonProvider = newProvider;
    }

    public void SetColor(JointType jointType, Color color) {
        throw new NotImplementedException();
    }

    public void SetColor(ComparisonFrameData comparison) {
        throw new NotImplementedException();
    }

    private void PositionSkeleton(Skeleton s) {
        Vector3 torsoPos;
        if (Bound) {
            torsoPos = Quaternion.Euler(0f, 180f, 0f) * BoundPos;
        } else {
            torsoPos = Quaternion.Euler(0f, 180f, 0f) * (0.001f * s.GetJoint(JointType.Torso).ToVector3());
        }
        transform.position = torsoPos;
    }

    private void RotateBones(Skeleton s) {
        foreach (RiggedModelJoint riggedModelJoint in ModelJoints) {
            Joint joint = s.GetJoint(riggedModelJoint.JointType);
            Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * riggedModelJoint.BaseRotOffset;
            riggedModelJoint.Bone.rotation = jointOrientation;
        }
    }

    private RiggedModelJoint FindJoint(JointType type) {
        return ModelJoints.First(m => m.JointType == type);
    }
}
