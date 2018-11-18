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

    private MeshRenderer cylinderRenderer;

    public void Colorize(Color c) {
        cylinderRenderer.material.color = c;
    }

    // ReSharper disable once ParameterHidesMember
    public void InitStudent(GameObject cylinder) {
        this.cylinder = cylinder;
        cylinderRenderer = this.cylinder.GetComponent<MeshRenderer>();
        if (cylinder != null) {
            ArrangeCylinder();
        }
    }

    public void UpdateAngle(Skeleton s) {
        Joint joint = s.GetJoint(JointType);
        Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * BaseRotOffset;
        Bone.rotation = jointOrientation;

        if (cylinder != null) {
            //cylinder.transform.localScale = boneRenderer.bounds.size;

            //cylinder.transform.localScale = Bone.localScale;
            //cylinder.transform.position = Bone.position;
            //cylinder.transform.rotation = jointOrientation;

            ArrangeCylinder();
        }
    }

    private void ArrangeCylinder() {
        cylinder.transform.localScale = 0.002f * Bone.lossyScale;
        //cylinder.transform.localScale = boneRenderer.bounds.size;
        cylinder.transform.position = Bone.position;
        cylinder.transform.rotation = Bone.rotation;
    }
}
