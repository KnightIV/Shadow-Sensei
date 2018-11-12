using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;

public interface IReadOnlyTechnique {
    
    Skeleton[] ReadTechniqueFrames { get; }
    Skeleton[] ReadUserAttemptFrames { get; }
}
