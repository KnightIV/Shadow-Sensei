using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RiggedAvatar : MonoBehaviour, IAvatar {

    public ISkeletonProvider SkeletonProvider { get; protected set; }

    void Start() {
        SkeletonProvider = CurrentUserTracker.Instance;
    }

    void FixedUpdate() {
        ProcessSkeleton(SkeletonProvider.CurSkeleton);
    }

    public void SwapSkeletonProvider(ISkeletonProvider newProvider) {
        SkeletonProvider = newProvider;
    }

    private void ProcessSkeleton(Skeleton s) {
        Vector3 torsoPos = Quaternion.Euler(0f, 180f, 0f) * (0.001f * s.GetJoint(JointType.Torso).ToVector3());
        transform.position = torsoPos;
    }
}
