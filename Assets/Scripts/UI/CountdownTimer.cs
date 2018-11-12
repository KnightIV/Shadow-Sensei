using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class CountdownStepEvent : UnityEvent<float> { }

[Serializable]
public class CountdownFinishedEvent : UnityEvent { }

public class CountdownTimer : MonoBehaviour {

    public bool IsRunning;
    public int StartCount;
    public TextMeshProUGUI CountdownText;
    public CountdownStepEvent StepEvent;
    public CountdownFinishedEvent FinishedEvent;

    public string Text {
        get { return CountdownText?.text; }
        set {
            if (CountdownText != null) {
                CountdownText.text = value;
            }
        }
    }

    private float curCount;

    void Start() {
        curCount = StartCount;
    }

    void FixedUpdate() {
        if (IsRunning) {
            if (curCount > 0.0f) {
                curCount -= Time.fixedDeltaTime;
                StepEvent?.Invoke(curCount);
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
    }

    public void ResetAndRun() {
        Reset();
        SetRunning(true);
    }

    public void Reset() {
        curCount = StartCount;
        UpdateText();
    }

    private void UpdateText() {
        Text = Mathf.RoundToInt(curCount).ToString();
    }
}
