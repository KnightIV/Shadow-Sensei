using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;
using UnityEngine.UI;

public class TrainingFinished : MonoBehaviour {

    public NativeAvatar UserNativeAvatar;
    public NativeAvatar TechniqueNativeAvatar;
    public Playback UserPlayback, TechniquePlayback;
    public AnimationCurveProvider CurveProvider;
    public ScoreSlider FrameScoreBar;

    public Text FrameScoreText;

    private SkeletonComparer comparer;
    
	void Start() { 
		comparer = new AngleSkeletonComparer(CurveProvider.Curve);
	}
	
	void FixedUpdate () {
	    ComparisonFrameData comparison = comparer.Compare(UserNativeAvatar.CurSkeleton, TechniqueNativeAvatar.CurSkeleton);
	    JointTracker[] userTrackers = UserNativeAvatar.JointTrackers;

	    foreach (JointTracker tracker in userTrackers) {
	        float jointScore = comparison[tracker.JointType];

	        tracker.Color.g = jointScore;
	        tracker.Color.b = jointScore;
	    }

	    float totalScore = comparison.TotalScore;
        FrameScoreBar.UpdateScore(totalScore);
	    //Color textColor;
	    //if (totalScore > 0.5f) {
	    //    textColor = Color.Lerp(Color.yellow, Color.green, (totalScore - 0.5f) * 2);
	    //} else {
	    //    textColor = Color.Lerp(Color.red, Color.yellow, totalScore * 2);
	    //}

	    //FrameScoreText.text = $"{comparison.TotalScorePercent:F2}%";
	    //FrameScoreText.color = textColor;
	}

    public void SetupTrainingFinished(IEnumerable<Skeleton> recordedFrames) {
        VariableHolder.RecordedSkeletonFrames = recordedFrames.Select(s => (SerializableSkeleton) s).ToArray();
        UserPlayback.RefreshSkeletonFrames();
        UserNativeAvatar.SwapSkeletonProvider(UserPlayback);
        TechniqueNativeAvatar.SwapSkeletonProvider(TechniquePlayback);

        int minEndFrame = Mathf.Min(TechniquePlayback.EndFrame, UserPlayback.EndFrame);
        UserPlayback.SetStartEndFrames(0, minEndFrame);
        TechniquePlayback.SetStartEndFrames(0, minEndFrame);
    }
}
