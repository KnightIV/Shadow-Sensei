using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ComparisonData {

    public ComparisonFrameData[] FrameComparisons;

    public float TotalScore {
        get {
            if (FrameComparisons == null) {
                throw new InvalidOperationException("FrameComparisons is null");
            }

            if (FrameComparisons.Length == 0) {
                Debug.Log("FrameComparisons is empty");
            }
            return FrameComparisons.Sum(f => f.TotalScore) / FrameComparisons.Length;
        }
    }

    public float TotalScorePercent {
        get { return TotalScore.ToPercent(); }
    }
}
