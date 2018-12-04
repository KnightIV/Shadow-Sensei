using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Mathf = UnityEngine.Mathf;

[Serializable]
public class CountdownStepEvent : UnityEvent<float> { }

[Serializable]
public class CountdownWholeStepEvent : UnityEvent<int> { }

[Serializable]
public class CountdownFinishedEvent : UnityEvent { }

public class CountdownTimer : MonoBehaviour {

    public bool IsRunning;
    public int StartCount;
    public TextMeshProUGUI CountdownText;
    public CountdownStepEvent StepEvent;
    public CountdownWholeStepEvent WholeStepEvent;
    public CountdownFinishedEvent FinishedEvent;

    public AudioSource Clip3, Clip2, Clip1, ClipGo;

    public string Text {
        get { return CountdownText?.text; }
        set {
            if (CountdownText != null) {
                CountdownText.text = value;
            }
        }
    }

    private float curCount;
    private int lastIntCount;

    void Start() {
        curCount = StartCount;
        lastIntCount = Mathf.RoundToInt(curCount);
    }

    void FixedUpdate() {
        if (IsRunning) {
            if (curCount > 0.0f) {
                curCount -= Time.fixedDeltaTime;
                StepEvent?.Invoke(curCount);

                if (lastIntCount > Mathf.RoundToInt(curCount)) {
                    lastIntCount = Mathf.RoundToInt(curCount);
                    WholeStepEvent?.Invoke(lastIntCount);
                }
            } else {
                curCount = 0.0f;
                IsRunning = false;
                FinishedEvent?.Invoke();
            }

            UpdateText();
        }
    }

    public void SetRunning(bool isRunning) {
        IsRunning = isRunning;

        if (IsRunning) {
            WholeStepEvent?.Invoke(lastIntCount);
        }
    }

    public void ResetAndRun() {
        Reset();
        SetRunning(true);
    }

    public void Reset() {
        curCount = StartCount;
        UpdateText();
    }

    public void AudioPlay(int count) {
        switch (count) {
            case 3:
                Clip3.Play();
                break;

            case 2:
                Clip2.Play();
                break;

            case 1:
                Clip1.Play();
                break;

            case 0:
                ClipGo.Play();
                break;
        }
    }

    private void UpdateText() {
        Text = Mathf.RoundToInt(curCount).ToString();
    }
}
