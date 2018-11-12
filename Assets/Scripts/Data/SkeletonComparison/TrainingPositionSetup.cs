using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPositionSetup : MonoBehaviour {

    public SkeletonComparer Comparer;
    public CountdownTimer CountdownTimer;
    public NativeAvatar UserNativeAvatar;
    public Playback UserPlayback;
    public NativeAvatar TechniqueNativeAvatar;
    public AnimationCurveProvider CurveProvider;
    public ScoreSlider ScoreBar;

    private bool isEnabled;

    #region Debug

    [SerializeField] private Text scoreDebugText;

    #endregion

    void Start() {
        Comparer = new AngleSkeletonComparer(CurveProvider.Curve);
    }

    void FixedUpdate() {
        if (isEnabled) {
            Skeleton userSkeleton = CurrentUserTracker.CurrentSkeleton;
            float totalScore = 0;

            if (userSkeleton != null) {
                Skeleton techniqueSkeleton = TechniqueNativeAvatar.CurSkeleton;

                ComparisonFrameData result = Comparer.Compare(userSkeleton, techniqueSkeleton);

                for (int i = 0; i < UserNativeAvatar.JointTrackers.Length; i++) {
                    JointTracker tracker = UserNativeAvatar.JointTrackers[i];
                    tracker.Color.g = result[tracker.JointType];
                    tracker.Color.b = result[tracker.JointType];

                    if (result[tracker.JointType] < 0) {
                        Debug.Log(result[tracker.JointType]);
                    }
                }

                totalScore = result.TotalScore;

                if (1 - totalScore < 0.1f) {
                    CountdownTimer.SetRunning(true);
                } else {
                    CountdownTimer.SetRunning(false);
                    CountdownTimer.Reset();
                    CountdownTimer.Text = "Into Position";
                }
            }

            ScoreBar.UpdateScore(totalScore);
        }
    }

    public void SetEnabled(bool enable) {
        isEnabled = enable;

        if (isEnabled) {
            UserPlayback.RefreshSkeletonFrames();
            TechniqueNativeAvatar.SwapSkeletonProvider(UserPlayback);
        }
    }
}
