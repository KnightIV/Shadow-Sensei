using System.Collections;
using System.Collections.Generic;
using nuitrack;
using UnityEngine;
using UnityEngine.UI;

public class TrainingPositionSetup : MonoBehaviour {

    public NativeAvatar UserNativeAvatar;
    public NativeAvatar TechniqueNativeAvatar;
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

    void Start() {
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

                //foreach (KeyValuePair<JointType, float> resultScore in result.JointScores) {
                //    JointType type = resultScore.Key;
                //    float score = resultScore.Value;

                //    UserAvatar.SetColor(type, new Color(1, score, score));
                //}

                //for (int i = 0; i < UserAvatar.JointTrackers.Length; i++) {
                //    JointTracker tracker = UserAvatar.JointTrackers[i];
                //    tracker.Color.g = result[tracker.JointType];
                //    tracker.Color.b = result[tracker.JointType];

                //    if (result[tracker.JointType] < 0) {
                //        Debug.Log(result[tracker.JointType]);
                //    }
                //}

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
            TechniquePlayback.RefreshSkeletonFrames();
            TechniqueAvatar.SwapSkeletonProvider(TechniquePlayback);
        }
    }
}
