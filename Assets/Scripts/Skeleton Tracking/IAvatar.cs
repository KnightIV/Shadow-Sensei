using nuitrack;
using UnityEngine;

public interface IAvatar : ISkeletonProvider {

    ISkeletonProvider SkeletonProvider { get; }
    bool DefaultToUserTracker { get; set; }

    void SwapSkeletonProvider(ISkeletonProvider newProvider);
    void SetColor(JointType jointType, Color color);
    void SetColor(ComparisonFrameData comparison);
    void SetEnabled(bool isEnabled);
}
