using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class PlaybackStepEvent : UnityEvent<SerializableSkeleton> { }

[Serializable]
public class PlaybackFrameStepEvent : UnityEvent<int> { }

public class Playback : MonoBehaviour, ISkeletonProvider {

    public Slider PlaybackSlider;
    public bool IsPlaying, IsSliderMoving;
    public int StartFrame, CurFrame, EndFrame;
    public IList<SerializableSkeleton> SkeletonFrames;
    public DoubleValueSlider TrimSlider;
    public UnityEvent OnFinish;
    public PlaybackStepEvent OnUpdate;
    public PlaybackFrameStepEvent OnUpdateFrame;

    public SerializableSkeleton CurSkeleton => SkeletonFrames?[CurFrame];

    void Start() {
        RefreshSkeletonFrames();
    }

    void FixedUpdate() {
        if (IsSliderMoving) {
            UpdateFrame();
        } else if (IsPlaying) {
            CurFrame = Mathf.Clamp(++CurFrame, StartFrame, EndFrame);
            OnUpdate?.Invoke(CurSkeleton);
            OnUpdateFrame?.Invoke(CurFrame);

            if (PlaybackSlider != null) {
                PlaybackSlider.value = CurFrame;
            }

            if (CurFrame == EndFrame) {
                IsPlaying = false;
                OnFinish?.Invoke();
            }
        }
    }

    public void RefreshSkeletonFrames() {
        SkeletonFrames = VariableHolder.RecordedSkeletonFrames;

        if (SkeletonFrames != null) {
            StartFrame = 0;
            CurFrame = StartFrame;
            EndFrame = SkeletonFrames.Count - 1;

            if (PlaybackSlider != null) {
                PlaybackSlider.minValue = StartFrame;
                PlaybackSlider.maxValue = EndFrame;
                PlaybackSlider.value = CurFrame;
            }

            if (TrimSlider != null) {
                TrimSlider.MinValue = StartFrame;
                TrimSlider.MaxValue = EndFrame;

                TrimSlider.ResetToMaxRange();
            }

            Debug.Log("SkeletonFrames successfully loaded");
        } else {
            Debug.Log("SkeletonFrames couldn't be loaded");
        }
    }

    public void TogglePlay() {
        SetPlaying(!IsPlaying);
    }

    public void SetPlaying(bool isPlaying) {
        IsPlaying = isPlaying;

        if (IsPlaying && CurFrame == EndFrame) {
            CurFrame = StartFrame;
        }
    }

    public void Reset() {
        CurFrame = StartFrame;
        if (PlaybackSlider != null) {
            PlaybackSlider.value = StartFrame;
            UpdateFrame();
        }
    }

    public void UpdateFrame() {
        PlaybackSlider.value = Mathf.Max(StartFrame, PlaybackSlider.value);
        PlaybackSlider.value = Mathf.Min(EndFrame, PlaybackSlider.value);

        CurFrame = (int) PlaybackSlider.value;
    }

    public void SetStartEndFrames(float start, float end) {
        int prevStartFrame = StartFrame;
        int prevEndFrame = EndFrame;

        StartFrame = (int) start;
        EndFrame = (int) end;

        if (Mathf.Abs(start - prevStartFrame) <= 0.0000001f) {
            // EndFrame changed
            CurFrame = EndFrame;
        } else if (Mathf.Abs(end - prevEndFrame) <= 0.0000001f) {
            // StartFrame changed
            CurFrame = StartFrame;
        }

        if (PlaybackSlider != null) {
            PlaybackSlider.value = CurFrame;
        }
    }
}
