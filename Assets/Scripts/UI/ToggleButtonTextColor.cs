using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TextMeshProUGUI = TMPro.TextMeshProUGUI;

public class ToggleButtonTextColor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

    public Color DefaultColor, HighlightedColor, ClickedColor;

    private TextMeshProUGUI buttonText;
    
	void Start () {
	    buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
	}

    public void OnPointerDown(PointerEventData eventData) {
        buttonText.color = HighlightedColor;
    }

    public void OnPointerUp(PointerEventData eventData) {
        buttonText.color = DefaultColor;
    }

    public void OnPointerClick(PointerEventData eventData) {
        buttonText.color = ClickedColor;
    }
}
