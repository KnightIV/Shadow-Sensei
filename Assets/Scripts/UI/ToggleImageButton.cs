using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImageButton : MonoBehaviour {

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite toggledSprite;

    private bool toggled;
    private Image buttonImage;
    
	void Start () {
	    buttonImage = gameObject.GetComponent<Image>();
	    buttonImage.sprite = defaultSprite;
	}

    public void Toggle() {
        toggled = !toggled;
        UpdateImage();
    }

    public void SetToggled(bool isToggled) {
        toggled = isToggled;
        UpdateImage();
    }

    private void UpdateImage() {
        buttonImage.sprite = toggled ? toggledSprite : defaultSprite;
    }
}
