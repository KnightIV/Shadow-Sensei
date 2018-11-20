using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingActions : MonoBehaviour {

    public NativeAvatar PlaybackNativeAvatar, UserNativeAvatar;
    public RiggedAvatar PlaybackRiggedAvatar, UserRiggedAvatar;

    public Playback Playback, FinishedPlayback;
    public AnimationCurveProvider CurveProvider;
    public ScoreSlider FinalScoreBar;

    private Technique technique;
    private SkeletonComparer comparer;
    private IAvatar PlaybackAvatar, UserAvatar;

    void Start() {
        PlaybackAvatar = PlaybackRiggedAvatar as IAvatar ?? PlaybackNativeAvatar;
        UserAvatar = UserRiggedAvatar as IAvatar ?? UserNativeAvatar;
    }

    public void Init(Technique t) {
        technique = t;
        comparer = SkeletonComparer.GetComparer(CurveProvider.Curve);

        VariableHolder.RecordedSkeletonFrames = t.TechniqueFrames;
        Playback.RefreshSkeletonFrames();
        FinishedPlayback.RefreshSkeletonFrames();

        PlaybackAvatar.SwapSkeletonProvider(Playback);
        UserAvatar.SwapSkeletonProvider(CurrentUserTracker.Instance);

        PlaybackAvatar.SetEnabled(true);
        UserAvatar.SetEnabled(true);
    }

    public void FinishTraining(IEnumerable<Skeleton> recordedFrames) {
        technique.UserAttemptFrames = recordedFrames.Select(s => (SerializableSkeleton) s).ToArray();

        ComparisonData data = comparer.Evaluate(technique);

        float totalScore = data.TotalScore;
        technique.LastAttemptedDateTime = DateTime.Now;

        TechniqueFileHelper.RegisterAttempt(technique, totalScore);
        FinalScoreBar.UpdateScore(totalScore);
    }
}

