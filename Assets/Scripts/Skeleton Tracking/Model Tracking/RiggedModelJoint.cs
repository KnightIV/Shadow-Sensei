using System;
using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;

[Serializable]
public class RiggedModelJoint {

    public ISkeletonProvider SkeletonProvider { get; set; }
    public string Name => Bone.name;

    public Transform Bone;
    public JointType JointType;

    [HideInInspector] public Quaternion BaseRotOffset;

    [SerializeField] private GameObject cylinder;

    private Renderer boneRenderer;

    void Start() {
        boneRenderer = Bone.GetComponent<Renderer>();

        if (boneRenderer == null) {
            Debug.Log($"No renderer found for {Name}");
        }
    }

    void FixedUpdate() {
        if (SkeletonProvider?.CurSkeleton != null) {
            //UpdateAngle(SkeletonProvider.CurSkeleton);
        }
    }

    public void Colorize(Color c) {

    }

    public void InitStudent(GameObject cylinder) {
        this.cylinder = cylinder;
    }

    public void UpdateAngle(Skeleton s) {
        Joint joint = s.GetJoint(JointType);
        Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * BaseRotOffset;
        Bone.rotation = jointOrientation;

        if (cylinder != null) {
            cylinder.transform.localScale = boneRenderer.bounds.size;
            cylinder.transform.position = Bone.position;
            cylinder.transform.rotation = jointOrientation;
        }
    }
}
