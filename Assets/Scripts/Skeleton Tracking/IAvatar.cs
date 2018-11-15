using nuitrack;
using UnityEngine;

public interface IAvatar : ISkeletonProvider {

    ISkeletonProvider SkeletonProvider { get; }

    void SwapSkeletonProvider(ISkeletonProvider newProvider);
    void SetColor(JointType jointType, Color color);
    void SetColor(ComparisonFrameData comparison);
}
