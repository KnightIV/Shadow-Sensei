using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;

[Serializable]
public class ComparisonFrameData {

    public static readonly ComparisonFrameData Zero = new ComparisonFrameData {
        JointScores = new Dictionary<JointType, float> {
            { JointType.None, 0 }
        }
    };

    public Dictionary<JointType, float> JointScores;

    public float TotalScore {
        get {
            //return JointScores.Values.Sum() / JointScores.Count;

            float weightedValueSum = JointScores.Sum(p => p.Value * DistanceFromLowestScore(p.Key));
            float weightSum = JointScores.Keys.Sum(k => DistanceFromLowestScore(k));

            return weightedValueSum / weightSum;
        }
    }

    public float TotalScorePercent {
        get { return TotalScore.ToPercent(); }
    }

    public float this[JointType key] {
        get {
            float score;
            JointScores.TryGetValue(key, out score);
            return score;
        }

        set {
            if (!JointScores.ContainsKey(key)) {
                JointScores.Add(key, value);
            } else {
                JointScores[key] = value;
            }
        }
    }

    public ComparisonFrameData() {
        JointScores = new Dictionary<JointType, float>();
    }

    private int DistanceFromLowestScore(JointType type) {
        List<JointType> orderedTypes = JointScores.OrderBy(p => p.Value).Select(p => p.Key).ToList();

        return orderedTypes.Count - orderedTypes.IndexOf(type);
    }
}
