using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkeletonProvider {
    
    SerializableSkeleton CurSkeleton { get; }
}
