using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaybackSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public Playback[] AssociatedPlaybacks;

    public void OnPointerDown(PointerEventData eventData) {
        if (AssociatedPlaybacks != null) {
            foreach (Playback playback in AssociatedPlaybacks) {
                playback.IsSliderMoving = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (AssociatedPlaybacks != null) {
            foreach (Playback playback in AssociatedPlaybacks) {
                playback.IsSliderMoving = false;
            }
        }
    } 
}
