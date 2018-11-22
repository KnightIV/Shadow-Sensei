using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TextMeshProUGUI = TMPro.TextMeshProUGUI;

public class ToggleButtonTextColor : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler {

    public Color Color {
        set { buttonText.color = value; }
    }

    public Color DefaultColor, HighlightedColor, ClickedColor;

    private TextMeshProUGUI buttonText;
    
	void Start () {
	    buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

	    Color = DefaultColor;
	}

    public void OnPointerEnter(PointerEventData eventData) {
        Color = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        Color = DefaultColor;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Color = ClickedColor;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Color = DefaultColor;
    }
}
