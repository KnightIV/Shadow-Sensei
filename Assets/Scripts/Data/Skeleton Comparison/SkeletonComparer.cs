using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using nuitrack;

[Serializable]
public abstract class SkeletonComparer {

    protected AnimationCurve curve;
    protected object mutex = new object();

    protected SkeletonComparer(AnimationCurve curve) {
        this.curve = curve;
    }

    public ComparisonData Evaluate(IReadOnlyTechnique t) {
        List<ComparisonFrameData> comparisonFrameData = new List<ComparisonFrameData>();

        Skeleton[] technique = t.ReadTechniqueFrames;
        Skeleton[] userAttempt = t.ReadUserAttemptFrames;

        int maxLength = technique.Length;

        if (technique.Length != userAttempt.Length) {
            Debug.Log($"tech: {technique.Length} | user: {userAttempt.Length}");
            maxLength = Mathf.Min(technique.Length, userAttempt.Length);
        }

        Parallel.For(0, maxLength, i => {
            ComparisonFrameData comparison = Compare(technique[i], userAttempt[i]);

            lock (comparisonFrameData) {
                comparisonFrameData.Add(comparison);
            }
        });

        ComparisonData data = new ComparisonData { FrameComparisons = comparisonFrameData.ToArray() };
        return data;
    }

    public abstract ComparisonFrameData Compare(Skeleton s1, Skeleton s2);
}
