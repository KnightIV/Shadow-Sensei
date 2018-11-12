using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TechniqueMetaData {

    public string TechniqueName, UserName;
    public long LastAttemptedTicks;
    public float BestScore, LastScore;

    public DateTime LastAttemptedDateTime {
        get { return new DateTime(LastAttemptedTicks); }
    }

    public float BestScorePercent {
        get { return BestScore * 100; }
    }

    public float LastScorePercent {
        get { return LastScore * 100; }
    }

    public static implicit operator TechniqueMetaData(Technique t) {
        return new TechniqueMetaData {
            TechniqueName = t.TechniqueName,
            UserName = t.UserName,
            LastAttemptedTicks = t.LastAttemptedDateTime.Ticks,
            BestScore = t.Score,
            LastScore = t.Score
        };
    }
}
