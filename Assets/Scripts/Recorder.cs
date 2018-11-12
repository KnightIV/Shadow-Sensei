using System;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RecordFinishEvent : UnityEvent<IEnumerable<Skeleton>> {}

public class Recorder : MonoBehaviour {
    
    public bool Recording;
    public ISkeletonProvider SkeletonProvider;
    public RecordFinishEvent OnStopRecording;
    public List<SerializableSkeleton> SkeletonFrames = new List<SerializableSkeleton>();

    void Start() {
        SkeletonProvider = SkeletonProvider ?? CurrentUserTracker.Instance;
    }

    void FixedUpdate() {
        if (Recording) {
            SerializableSkeleton skeleton = SkeletonProvider.CurSkeleton;
            SkeletonFrames.Add(skeleton);
        }
    }

    public void ToggleRecord() {
        Recording = !Recording;
        if (Recording) {
            SkeletonFrames.Clear();
        } else {
            OnStopRecording?.Invoke(SkeletonFrames.Select(s => (Skeleton) s).ToArray());
        }
    }

    public void StartRecording() {
        Recording = true;
    }

    public void StopRecording() {
        Recording = false;
        OnStopRecording?.Invoke(SkeletonFrames.Select(s => (Skeleton) s).ToArray());
    }

    public void ClearRecording() {
        SkeletonFrames.Clear();
    }
}
