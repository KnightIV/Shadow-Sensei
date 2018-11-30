using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPositionSetup : MonoBehaviour {

    public const float MIN_SCORE_THRESHOLD = 0.1f;

    public NativeAvatar UserNativeAvatar, TechniqueNativeAvatar;
    public RiggedAvatar UserRiggedAvatar, TechniqueRiggedAvatar;

    public SkeletonComparer Comparer;
    public CountdownTimer CountdownTimer;
    public Playback TechniquePlayback;
    public AnimationCurveProvider CurveProvider;
    public ScoreSlider ScoreBar;

    private bool isEnabled;
    private IAvatar UserAvatar, TechniqueAvatar;

    #region Debug

    [SerializeField] private Text scoreDebugText;

    #endregion

    void Awake() {
        Comparer = SkeletonComparer.GetComparer(CurveProvider.Curve);
        UserAvatar = UserRiggedAvatar as IAvatar ?? UserNativeAvatar;
        TechniqueAvatar = TechniqueRiggedAvatar as IAvatar ?? TechniqueNativeAvatar;
    }

    void FixedUpdate() {
        if (isEnabled) {
            Skeleton userSkeleton = CurrentUserTracker.CurrentSkeleton;
            float totalScore = 0;

            if (userSkeleton != null) {
                Skeleton techniqueSkeleton = TechniqueAvatar.CurSkeleton;
                ComparisonFrameData result = Comparer.Compare(userSkeleton, techniqueSkeleton);

                UserAvatar.SetColor(result);
                totalScore = result.TotalScore;

                if (1 - totalScore < MIN_SCORE_THRESHOLD) {
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
            TechniquePlayback.RefreshSkeletonFrames();
            TechniqueAvatar.SwapSkeletonProvider(TechniquePlayback);
            CountdownTimer.Text = "Into Position";
        }
    }
}
