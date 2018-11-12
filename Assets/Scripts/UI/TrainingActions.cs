using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingActions : MonoBehaviour {

    public NativeAvatar PlaybackAvatar;
    public NativeAvatar UserAvatar;
    public Playback Playback, FinishedPlayback;
    public AnimationCurveProvider CurveProvider;
    public ScoreSlider FinalScoreBar;

    public Text FinishedText;

    private Technique technique;
    private SkeletonComparer comparer;

    public void Init(Technique t) {
        technique = t;
        comparer = new AngleSkeletonComparer(CurveProvider.Curve);

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

        //FinishedText.text = $"Total Score: {data.TotalScorePercent:F2}%";

        //Debug.Log(FinishedText.text);

        //Color textColor;
        //if (totalScore > 0.5f) {
        //    textColor = Color.Lerp(Color.yellow, Color.green, (totalScore - 0.5f) * 2);
        //} else {
        //    textColor = Color.Lerp(Color.red, Color.yellow, totalScore * 2);
        //}

        //FinishedText.color = textColor;
    }
}

