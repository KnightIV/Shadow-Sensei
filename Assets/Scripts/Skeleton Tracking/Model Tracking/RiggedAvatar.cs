using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class RiggedAvatar : MonoBehaviour, IAvatar {

    public static readonly Color INCORRECT = new Color(1, 0, 0, 0.9f);
    public static readonly Color MIDPOINT = new Color(0.5f, 0, 0, 0.7f);
    public static readonly Color CORRECT = Color.clear;

    [Header("Rigged Model")] public RiggedModelJoint[] ModelJoints;
    public bool Bound;
    public Vector3 BoundPos;

    public ISkeletonProvider SkeletonProvider { get; protected set; }
    public SerializableSkeleton CurSkeleton => SkeletonProvider.CurSkeleton;

    [SerializeField] private bool isEnabled, isStudent, training;
    [SerializeField] private GameObject scorerPrefab;

    [SerializeField] private bool defaultToUserTracker;
    public bool DefaultToUserTracker {
        get { return defaultToUserTracker; }
        set { defaultToUserTracker = value; }
    }

    void Start() {
        if (DefaultToUserTracker) {
            SkeletonProvider = CurrentUserTracker.Instance;
        }

        foreach (RiggedModelJoint modelJoint in ModelJoints) {
            modelJoint.BaseRotOffset = modelJoint.Bone.rotation;
            modelJoint.SkeletonProvider = this;

            if (isStudent) {
                GameObject cylinder = GameObject.Find(modelJoint.Name + " - score") ?? Instantiate(scorerPrefab);
                cylinder.name = modelJoint.Name + " - score";
                modelJoint.InitStudent(cylinder);
            }
        }

        SetEnabled(isEnabled);
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
        FindJoint(jointType)?.Colorize(color);
    }

    public void SetColor(ComparisonFrameData comparison) {
        if (training) {
            foreach (KeyValuePair<JointType, float> result in comparison.JointScores) {
                JointType type = result.Key;
                float score = result.Value;

                Color color;
                if (score < 0.5f) {
                    color = Color.Lerp(INCORRECT, MIDPOINT, score * 2);
                } else {
                    color = Color.Lerp(MIDPOINT, CORRECT, (score - 0.5f) * 2);
                }

                SetColor(type, color);
            }
        }
    }

    // ReSharper disable once ParameterHidesMember
    public void SetEnabled(bool isEnabled) {
        this.isEnabled = isEnabled;

        foreach (Renderer rendererComponent in gameObject.GetComponentsInChildren<Renderer>()) {
            rendererComponent.enabled = isEnabled;
        }
    }

    public void SetTraining(bool isTraining) {
        if (isStudent) {
            training = isTraining;

            foreach (RiggedModelJoint joint in ModelJoints) {
                joint.SetTraining(isTraining);
            }
        }
    }

    private void PositionSkeleton(Skeleton s) {
        Vector3 torsoPos;
        if (Bound) {
            torsoPos = BoundPos;
        } else {
            torsoPos = Quaternion.Euler(0f, 180f, 0f) * (0.001f * s.GetJoint(JointType.Torso).ToVector3());
        }
        transform.position = torsoPos;
    }

    private void RotateBones(Skeleton s) {
        foreach (RiggedModelJoint riggedModelJoint in ModelJoints) {
            riggedModelJoint.UpdateAngle(s);
        }
    }

    private RiggedModelJoint FindJoint(JointType type) {
        return ModelJoints.FirstOrDefault(m => m.JointType == type);
    }
}
