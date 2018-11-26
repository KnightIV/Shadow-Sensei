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

    private MeshRenderer scorerRenderer;

    public void Colorize(Color c) {
        scorerRenderer.material.color = c;
    }

    public void InitStudent(GameObject scorer) {
        scoreGameObject = scorer;
        scorerRenderer = scoreGameObject.GetComponent<MeshRenderer>();
        scoreGameObject.transform.parent = Bone;
        scoreGameObject.transform.localPosition = Vector3.zero;

        if (JointType == JointType.Waist) {
            scoreGameObject.transform.localScale *= 2;
        } else if (JointType == JointType.Torso) {
            scoreGameObject.transform.localScale *= 2.3f;
        }

        SetTraining(false);
    }

    public void SetTraining(bool isTraining) {
        Color c = isTraining ? scorerRenderer.material.color : Color.clear;
        Colorize(c);
    }

    public void UpdateAngle(Skeleton s) {
        Joint joint = s.GetJoint(JointType);
        Quaternion jointOrientation = Quaternion.Inverse(CalibrationInfo.SensorOrientation) * joint.ToQuaternionMirrored() * BaseRotOffset;
        Bone.rotation = jointOrientation;
    }
}
