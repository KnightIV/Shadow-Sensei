using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextButton : MonoBehaviour {

    public Text ButtonText;
    public string DefaultText, ToggledText;
    
    void Start() {
        ButtonText.text = DefaultText;
    }

    public void ToggleText() {
        if (ButtonText.text == DefaultText) {
            ButtonText.text = ToggledText;
        } else {
            ButtonText.text = DefaultText;
        }
    }
}
