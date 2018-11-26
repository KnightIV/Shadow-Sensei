using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickListener : MonoBehaviour, IPointerClickHandler {

    public delegate void OnClick();

    public event OnClick OnRightClick, OnLeftClick;

    public void OnPointerClick(PointerEventData eventData) {
        switch (eventData.button) {
            case PointerEventData.InputButton.Right:
                OnRightClick?.Invoke();
                break;

            case PointerEventData.InputButton.Left:
                OnLeftClick?.Invoke();
                break;
        }
    }
}
