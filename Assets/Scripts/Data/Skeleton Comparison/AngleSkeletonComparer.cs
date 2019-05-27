using System;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;
using Joint = nuitrack.Joint;
using Vector3 = UnityEngine.Vector3;

public class ScoreTracker {

    public float TotalScore { get; set; }
    public int Count { get; set; }

    public float AverageScore {
        get { return TotalScore / Count; }
    }
}

public class AngleSkeletonComparer : SkeletonComparer {

    protected static float SolveForAngleC(float a, float b, float c) {
        float cos = (a.Squared() + b.Squared() - c.Squared()) / (2 * a * b);
        float acos = Mathf.Acos(Mathf.Clamp(cos, -1, 1));

        return acos;
    }

    internal AngleSkeletonComparer(AnimationCurve curve) : base(curve) {}

    public override ComparisonFrameData Compare(Skeleton s1, Skeleton s2) {
        Dictionary<JointType, ScoreTracker> preliminaryScoreTrackers = new Dictionary<JointType, ScoreTracker>();
        ComparisonFrameData comparison = new ComparisonFrameData();

        foreach (JointType centerJoint in Enum.GetValues(typeof(JointType)).Cast<JointType>().ToArray()) {
            JointType[] adjacentJoints = GetAdjacentJoints(centerJoint);
            if (adjacentJoints != null) {
                Vector3 adjacentA1 = s1.GetJoint(adjacentJoints[0]).ToVector3();
                Vector3 center1 = s1.GetJoint(centerJoint).ToVector3();
                Vector3 adjacentB1 = s1.GetJoint(adjacentJoints[1]).ToVector3();

                Vector3 adjacentA2 = s2.GetJoint(adjacentJoints[0]).ToVector3();
                Vector3 center2 = s2.GetJoint(centerJoint).ToVector3();
                Vector3 adjacentB2 = s2.GetJoint(adjacentJoints[1]).ToVector3();

                Vector3 a1 = center1 - adjacentB1;
                Vector3 b1 = adjacentA1 - center1;
                Vector3 c1 = adjacentA1 - adjacentB1;

                Vector3 a2 = center2 - adjacentB2;
                Vector3 b2 = adjacentA2 - center2;
                Vector3 c2 = adjacentA2 - adjacentB2;

                float angle1 = SolveForAngleC(a1.magnitude, b1.magnitude, c1.magnitude);
                float angle2 = SolveForAngleC(a2.magnitude, b2.magnitude, c2.magnitude);

                float firstRatio = angle1 / angle2;
                float secondRatio = angle2 / angle1;

                float preliminary = Math.Min(firstRatio, secondRatio);
                float score = curve.Evaluate(preliminary);

                UpdateScore(centerJoint, score, preliminaryScoreTrackers);
                foreach (JointType adjacentJoint in adjacentJoints) {
                    UpdateScore(adjacentJoint, score, preliminaryScoreTrackers);
                }
            }
        }

        foreach (KeyValuePair<JointType, ScoreTracker> scoreTracker in preliminaryScoreTrackers) {
            JointType joint = scoreTracker.Key;
            ScoreTracker tracker = scoreTracker.Value;

            comparison[joint] = tracker.AverageScore;
        }

        return comparison;
    }

    protected void UpdateScore(JointType joint, float newScore, Dictionary<JointType, ScoreTracker> preliminaryScoreTrackers) {
        ScoreTracker tracker;

        if (!preliminaryScoreTrackers.TryGetValue(joint, out tracker)) {
            tracker = new ScoreTracker {
                TotalScore = newScore,
                Count = 1
            };

            preliminaryScoreTrackers.Add(joint, tracker);
        } else {
            tracker.TotalScore += newScore;
            tracker.Count++;

            //tracker.TotalScore = newScore;
        }
    }

    protected JointType[] GetAdjacentJoints(JointType center) {
        JointType[] adjacentJoints = new JointType[2];

        // TODO add missing cases for elements that the Kinect cannot detect
        switch (center) {
            case JointType.Neck:
                adjacentJoints[0] = JointType.Head;
                adjacentJoints[1] = JointType.RightCollar; // interchangeable with LeftCollar
                break;

            //case JointType.LeftCollar:
            //    adjacentJoints[0] = JointType.RightCollar;
            //    adjacentJoints[1] = JointType.LeftShoulder;
            //    break;
                
            case JointType.LeftShoulder:
                adjacentJoints[0] = JointType.LeftCollar;
                adjacentJoints[1] = JointType.LeftElbow;
                break;

            case JointType.LeftElbow:
                adjacentJoints[0] = JointType.LeftShoulder;
                adjacentJoints[1] = JointType.LeftWrist;
                break;

            case JointType.LeftWrist:
                adjacentJoints[0] = JointType.LeftElbow;
                adjacentJoints[1] = JointType.LeftHand;
                break;

            //case JointType.RightCollar:
            //    adjacentJoints[0] = JointType.LeftCollar;
            //    adjacentJoints[1] = JointType.RightShoulder;
            //    break;

            case JointType.RightShoulder:
                adjacentJoints[0] = JointType.RightCollar;
                adjacentJoints[1] = JointType.RightElbow;
                break;

            case JointType.RightElbow:
                adjacentJoints[0] = JointType.RightShoulder;
                adjacentJoints[1] = JointType.RightWrist;
                break;

            case JointType.RightWrist:
                adjacentJoints[0] = JointType.RightElbow;
                adjacentJoints[1] = JointType.RightHand;
                break;

            case JointType.Torso:
                adjacentJoints[0] = JointType.RightCollar; // interchangeable with LeftCollar
                adjacentJoints[1] = JointType.Waist;
                break;

            case JointType.Waist:
                adjacentJoints[0] = JointType.LeftHip;
                adjacentJoints[1] = JointType.RightHip;
                break;

            case JointType.LeftHip:
                adjacentJoints[0] = JointType.Waist;
                adjacentJoints[1] = JointType.LeftKnee;
                break;

            case JointType.LeftKnee:
                adjacentJoints[0] = JointType.LeftHip;
                adjacentJoints[1] = JointType.LeftAnkle;
                break;

            case JointType.LeftAnkle:
                adjacentJoints[0] = JointType.LeftKnee;
                adjacentJoints[1] = JointType.LeftFoot;
                break;

            case JointType.RightHip:
                adjacentJoints[0] = JointType.Waist;
                adjacentJoints[1] = JointType.RightKnee;
                break;

            case JointType.RightKnee:
                adjacentJoints[0] = JointType.RightHip;
                adjacentJoints[1] = JointType.RightAnkle;
                break;

            case JointType.RightAnkle:
                adjacentJoints[0] = JointType.RightKnee;
                adjacentJoints[1] = JointType.RightFoot;
                break;

            default:
                adjacentJoints = null;
                break;
        }

        return adjacentJoints;
    }
}
