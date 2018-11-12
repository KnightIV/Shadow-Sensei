using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;

[Serializable]
public class Technique : IReadOnlyTechnique {

    public string TechniqueName, UserName;
    public SerializableSkeleton[] TechniqueFrames;
    public SerializableSkeleton[] UserAttemptFrames;
    public DateTime LastAttemptedDateTime;
    public float Score;

    public Skeleton[] ReadTechniqueFrames => TechniqueFrames.Select(s => (Skeleton) s).ToArray();
    public Skeleton[] ReadUserAttemptFrames => UserAttemptFrames.Select(s => (Skeleton) s).ToArray();

    public Technique() { }

    public Technique(string techniqueName, SerializableSkeleton[] techniqueFrames) : this(techniqueName, techniqueFrames, null) { }

    public Technique(string techniqueName, SerializableSkeleton[] techniqueFrames, string userName) : this(techniqueName, techniqueFrames, null, userName) { }

    public Technique(string techniqueName, SerializableSkeleton[] techniqueFrames, SerializableSkeleton[] userAttemptFrames, string userName) {
        TechniqueName = techniqueName;
        TechniqueFrames = techniqueFrames;
        UserAttemptFrames = userAttemptFrames;
        UserName = userName;
    }
}
