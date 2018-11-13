using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class SlidersChanged : UnityEvent<float, float> { }

public class DoubleValueSlider : MonoBehaviour {

    public Slider MainSlider;
    public Slider CompanionSlider;
    public SlidersChanged OnSlidersChanged;

    public float LesserValue {
        get { return Mathf.Min(MainSlider.value, CompanionSlider.value); }
        set { MainSlider.value = value; }
    }

    public float GreaterValue {
        get { return Mathf.Max(MainSlider.value, CompanionSlider.value); }
        set { CompanionSlider.value = value; }
    }

    public float MinValue {
        get {
            if (MainSlider.minValue - CompanionSlider.minValue > 0.0000001) {
                throw new InvalidOperationException($"slider min values do not match: {MainSlider.minValue} and {CompanionSlider.minValue}");
            }
            return MainSlider.minValue;
        }
        set { CompanionSlider.minValue = MainSlider.value = value; }
    }

    public float MaxValue {
        get {
            if (MainSlider.maxValue - CompanionSlider.maxValue > 0.0000001) {
                throw new InvalidOperationException($"slider max values do not match: {MainSlider.maxValue} and {CompanionSlider.maxValue}");
            }
            return MainSlider.maxValue;
        }
        set { CompanionSlider.maxValue = MainSlider.maxValue = value; }
    }

    private float pixelsPerUnit;
    private RectTransform handle;

    void Awake() {
        handle = MainSlider.handleRect;

        MainSlider.onValueChanged.AddListener(OnSliderChanged);
        CompanionSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void FixedUpdate() {
        if (pixelsPerUnit <= 0.000001) {
            pixelsPerUnit = GetComponent<RectTransform>().rect.width;
            pixelsPerUnit = pixelsPerUnit / (MainSlider.maxValue - MainSlider.minValue);
        }

        MainSlider.fillRect.rotation = new Quaternion(0, 0, 0, 0);
        MainSlider.fillRect.pivot = new Vector2(handle.transform.parent.localPosition.x, MainSlider.fillRect.pivot.y);
        
        if (MainSlider.value >= CompanionSlider.value) {
            MainSlider.fillRect.Rotate(0, 0, 180);
        }

        float valueDifference = Mathf.Abs(LesserValue - GreaterValue);
        MainSlider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelsPerUnit * valueDifference);

        MainSlider.fillRect.localPosition = handle.localPosition;
    }

    private void OnSliderChanged(float _) {
        OnSlidersChanged?.Invoke(LesserValue, GreaterValue);
    }

    public void ResetToMaxRange() {
        LesserValue = MinValue;
        GreaterValue = MaxValue;
    }
}
