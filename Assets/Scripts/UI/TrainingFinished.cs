using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;
using UnityEngine.UI;

public class TrainingFinished : MonoBehaviour {

    public NativeAvatar UserNativeAvatar, TechniqueNativeAvatar;
    public RiggedAvatar TechniqueRiggedAvatar, UserRiggedAvatar;

    public Playback UserPlayback, TechniquePlayback;
    public AnimationCurveProvider CurveProvider;
    public ScoreSlider FrameScoreBar;

    public Text FrameScoreText;

    private SkeletonComparer comparer;
    private IAvatar UserAvatar, TechniqueAvatar;

    void Awake() {
        comparer = SkeletonComparer.GetComparer(CurveProvider.Curve);

        UserAvatar = UserRiggedAvatar as IAvatar ?? UserNativeAvatar;
        TechniqueAvatar = TechniqueRiggedAvatar as IAvatar ?? TechniqueNativeAvatar;
    }

    void FixedUpdate() {
        ComparisonFrameData comparison = comparer.Compare(UserAvatar.CurSkeleton, TechniqueAvatar.CurSkeleton);
        UserAvatar.SetColor(comparison);

        //foreach (KeyValuePair<JointType, float> result in comparison.JointScores) {
        //    JointType type = result.Key;
        //       float score = 
        //}

        //JointTracker[] userTrackers = UserNativeAvatar.JointTrackers;

        //foreach (JointTracker tracker in userTrackers) {
        //    float jointScore = comparison[tracker.JointType];

        //    tracker.Color.g = jointScore;
        //    tracker.Color.b = jointScore;
        //}

        float totalScore = comparison.TotalScore;
        FrameScoreBar.UpdateScore(totalScore);
    }

    public void SetupTrainingFinished(IEnumerable<Skeleton> recordedFrames) {
        VariableHolder.RecordedSkeletonFrames = recordedFrames.Select(s => (SerializableSkeleton) s).ToArray();
        UserPlayback.RefreshSkeletonFrames();
        UserAvatar.SwapSkeletonProvider(UserPlayback);
        TechniqueAvatar.SwapSkeletonProvider(TechniquePlayback);

        int minEndFrame = Mathf.Min(TechniquePlayback.EndFrame, UserPlayback.EndFrame);
        UserPlayback.SetStartEndFrames(0, minEndFrame);
        TechniquePlayback.SetStartEndFrames(0, minEndFrame);

        UserPlayback.Reset();
        TechniquePlayback.Reset();
    }
}
