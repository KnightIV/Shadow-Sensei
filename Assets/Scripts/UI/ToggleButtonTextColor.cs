using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TextMeshProUGUI = TMPro.TextMeshProUGUI;

public class ToggleButtonTextColor : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler {

    public Color Color {
        set {
            if (button.interactable) {
                foreach (TextMeshProUGUI text in buttonTexts) {
                    text.color = value;
                }
            }
        }
    }

    public Color DefaultColor, HighlightedColor, ClickedColor;

    private TextMeshProUGUI[] buttonTexts;
    private Button button;

    void Start() {
        buttonTexts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        button = gameObject.GetComponent<Button>();

        Color = DefaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Color = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Color = DefaultColor;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            Color = ClickedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        Color = DefaultColor;
    }
}
