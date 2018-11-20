using System;
using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;
using Vector3 = UnityEngine.Vector3;

[Serializable]
public class RiggedModelJoint {

    public ISkeletonProvider SkeletonProvider { get; set; }
    public string Name => Bone.name;

    public Transform Bone;
    public JointType JointType;

    [HideInInspector] public Quaternion BaseRotOffset;
    
    [SerializeField] private GameObject scoreGameObject;
    [SerializeField] private MeshRenderer scorerRenderer;

    public void Colorize(Color c) {
        scorerRenderer.material.color = c;
    }

    // ReSharper disable once ParameterHidesMember
    public void InitStudent(GameObject scoreGameObject) {
        this.scoreGameObject = scoreGameObject;
        scorerRenderer = this.scoreGameObject.GetComponent<MeshRenderer>();
        this.scoreGameObject.transform.parent = Bone;
        this.scoreGameObject.transform.localPosition = Vector3.zero;

        SetTraining(false);
    }

    public void SetTraining(bool isTraining) {
        //scoreGameObject.SetActive(isTraining);
        //scorerRenderer.enabled = isTraining;
        Color c = isTraining ? scorerRenderer.material.color : Color.clear;
        Colorize(c);
    }

    public void UpdateAngle(Skeleton s) {
        Joint joint = s.GetJoint(JointType);
        Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * BaseRotOffset;
        Bone.rotation = jointOrientation;

        //if (scoreGameObject != null) {
        //    //scoreGameObject.transform.localScale = boneRenderer.bounds.size;

        //    //scoreGameObject.transform.localScale = Bone.localScale;
        //    //scoreGameObject.transform.position = Bone.position;
        //    //scoreGameObject.transform.rotation = jointOrientation;

        //    ArrangeCylinder();
        //}
    }
}
