﻿using System;
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

    public AnimationCurve Curve {
        set { curve = value; }
    }

    protected object mutex = new object();

    public static SkeletonComparer GetComparer(AnimationCurve curve) {
        return new AngleSkeletonComparer(curve);
    }

    protected SkeletonComparer(AnimationCurve curve) {
        Curve = curve;
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
            ComparisonFrameData comparison;
            if (technique[i] == null || userAttempt[i] == null) {
                comparison = ComparisonFrameData.Zero;
            } else {
                comparison = Compare(technique[i], userAttempt[i]);
            }

            lock (comparisonFrameData) {
                comparisonFrameData.Add(comparison);
            }
        });

        ComparisonData data = new ComparisonData { FrameComparisons = comparisonFrameData.ToArray() };
        return data;
    }

    public abstract ComparisonFrameData Compare(Skeleton s1, Skeleton s2);
    
}
