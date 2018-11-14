using System;
using UnityEngine;

using nuitrack;
using Joint = nuitrack.Joint;
using Vector3 = UnityEngine.Vector3;

public class JointTracker : MonoBehaviour {

    public const float MINIMUM_CONFIDENCE = float.Epsilon;

    public ISkeletonProvider SkeletonProvider;
    public JointType JointType;
    public bool Bound;
    public Vector3 Origin;
    public JointType OffsetJointType;
    public NativeAvatar OwnerNativeAvatar;
    public Color Color;

    public bool Enabled { get; set; }

    private MeshRenderer meshRenderer;

    [SerializeField] private Vector3 actualPosition;

    void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        Color = meshRenderer.material.color;
    }

    void FixedUpdate() {
        Skeleton userSkeleton = SkeletonProvider.CurSkeleton;

        if (userSkeleton == null || !Enabled) {
            meshRenderer.enabled = false;
        } else {
            try {
                Joint joint = userSkeleton.GetJoint(JointType);
                actualPosition = joint.ToVector3();
                meshRenderer.enabled = joint.Confidence >= MINIMUM_CONFIDENCE;

                Vector3 jointVector = joint.ToVector3();
                Vector3 newPosition = 0.01f * jointVector;

                if (Bound) {
                    newPosition += Origin;

                    if (JointType == OffsetJointType) {
                        OwnerNativeAvatar.JointTrackerOffsetDistance = newPosition;
                        newPosition = Origin;
                    } else {
                        newPosition += Origin - OwnerNativeAvatar.JointTrackerOffsetDistance;
                    }
                }

                meshRenderer.material.color = Color;
                transform.position = newPosition;
            } catch (IndexOutOfRangeException) {
                meshRenderer.enabled = false;
            }
        }
    }
}
