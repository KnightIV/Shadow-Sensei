using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;

[Serializable]
public class ComparisonFrameData {

    public Dictionary<JointType, float> JointScores;

    public float TotalScore {
        get { return JointScores.Values.Sum() / JointScores.Count; }
    }

    public float TotalScorePercent {
        get { return TotalScore * 100; }
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
}
