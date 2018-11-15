using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using nuitrack;
using Vector3 = UnityEngine.Vector3;

public class NativeAvatar : MonoBehaviour, ISkeletonProvider, IAvatar {

    public GameObject PrefabJoint;
    public string JointNameModifier;
    public bool IsEnabled;
    public bool Bound;
    public float TorsoX, TorsoY, TorsoZ;
    public Vector3 JointTrackerOffsetDistance;
    public JointTracker[] JointTrackers;

    public ISkeletonProvider SkeletonProvider { get; protected set; }
    public SerializableSkeleton CurSkeleton => SkeletonProvider.CurSkeleton;

    void Start() {
        JointType[] typeJoint = Enum.GetValues(typeof(JointType)).Cast<JointType>().ToArray();
        JointTrackers = new JointTracker[typeJoint.Length];
        SkeletonProvider = CurrentUserTracker.Instance;

        Vector3 origin = new Vector3(TorsoX, TorsoY, TorsoZ);

        for (int i = 0; i < typeJoint.Length; i++) {
            string jointName = $"{typeJoint[i]} - {(String.IsNullOrEmpty(JointNameModifier) ? "Default" : JointNameModifier)}";
            GameObject createdJoint = GameObject.Find(jointName) ?? Instantiate(PrefabJoint);
            createdJoint.transform.SetParent(transform);
            createdJoint.name = jointName;

            JointTracker tracker = createdJoint.GetComponent<JointTracker>();
            tracker.OwnerNativeAvatar = this;
            tracker.Bound = Bound;
            tracker.Origin = origin;
            tracker.JointType = typeJoint[i];
            tracker.OffsetJointType = JointType.Waist;
            tracker.SkeletonProvider = SkeletonProvider;
            tracker.Enabled = IsEnabled;
            JointTrackers[i] = tracker;
        }
    }

    public void SwapSkeletonProvider(ISkeletonProvider provider) {
        SkeletonProvider = provider;

        for (int i = 0; i < JointTrackers.Length; i++) {
            JointTrackers[i].SkeletonProvider = provider;
        }
    }

    public void SetColor(JointType jointType, Color color) {
        JointTracker tracker = FindTracker(jointType);
        tracker.Color = color;
    }

    public void SetColor(ComparisonFrameData comparison) {
        foreach (KeyValuePair<JointType, float> resultScore in comparison.JointScores) {
            JointType type = resultScore.Key;
            float score = resultScore.Value;

            SetColor(type, new Color(1, score, score));
        }
    }

    // ReSharper disable once ParameterHidesMember
    public void SetEnabled(bool enabled) {
        for (int i = 0; i < JointTrackers.Length; i++) {
            JointTrackers[i].Enabled = enabled;
        }
    }

    private JointTracker FindTracker(JointType type) {
        return JointTrackers.First(t => t.JointType == type);
    }
}
