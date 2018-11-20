using System;
using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;
using Vector3 = UnityEngine.Vector3;

[Serializable]
public class NormalizingSkeletonComparer : SkeletonComparer {

    internal NormalizingSkeletonComparer(AnimationCurve curve) : base(curve) { }

    public override ComparisonFrameData Compare(Skeleton s1, Skeleton s2) {
        ComparisonFrameData comparisonData = new ComparisonFrameData();

        Skeleton uniformSkeleton1 = UniformSkeleton(s1);
        Skeleton uniformSkeleton2 = UniformSkeleton(s2);

        Joint[] joints1 = uniformSkeleton1.Joints;
        Joint[] joints2 = uniformSkeleton2.Joints;

        for (int i = 0; i < joints1.Length; i++) {
            Joint j1 = joints1[i];
            Joint j2 = joints2[i];

            if (j1.Type != j2.Type) {
                throw new InvalidOperationException($"wrong type of joints being compared: {j1.Type} != {j2.Type}");
            }

            Vector3 v1 = j1.ToVector3();
            Vector3 v2 = j2.ToVector3();

            Vector3 diff = v2 - v1;
            float preliminary = diff.magnitude;

            #region Different preliminary implementations

            //float preliminary = Mathf.Atan(v2.sqrMagnitude/v1.sqrMagnitude);
            //float preliminary = Mathf.Clamp(diff.magnitude, 0, 1);
            //float preliminary = v1 == v2 ? 1.0f : Vector3.Dot(v1, v2);
            //float preliminary = v1 == v2 ? 1.0f : Vector3.Dot(v1, v2);

            #endregion

            //float score = curve.Evaluate(preliminary);
            float score = curve.Evaluate(1 - preliminary);
            if (preliminary > 1.0f) {
                Debug.Log($"{j1.Type} : {preliminary}");
            }

            comparisonData[j1.Type] = score;
        }

        return comparisonData;
    }

    protected Skeleton UniformSkeleton(Skeleton s) {
        Vector3 originalTorsoPos = Vector3.zero;

        Joint[] uniformJoints = new Joint[s.Joints.Length];

        for (int i = 0; i < uniformJoints.Length; i++) {
            Joint joint = s.Joints[i];

            Vector3 jointVector = joint.ToVector3();
            Vector3 translatedJointVector = jointVector - originalTorsoPos;
            Vector3 normalizedVector = translatedJointVector.normalized;

            joint.Real = normalizedVector.ToNuitrackVector3();
            uniformJoints[i] = joint;
        }

        return new Skeleton(s.ID, uniformJoints);
    }
}
