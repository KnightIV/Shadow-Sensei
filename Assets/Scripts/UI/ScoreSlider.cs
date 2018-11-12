using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSlider : MonoBehaviour {

    private Slider scoreBar;
    private Image fill;

	void Start () {
	    scoreBar = gameObject.GetComponent<Slider>();
	    fill = gameObject.GetComponentsInChildren<Image>().First(f => f.name == "Fill");
	}

    public void UpdateScore(float score) {
        if (score > 1.0f || score < 0.0f) {
            throw new ArgumentOutOfRangeException(nameof(score));
        }

        scoreBar.value = Mathf.Lerp(scoreBar.minValue, scoreBar.maxValue, score);

        Color color;
        if (score > 0.5f) {
            color = Color.Lerp(Color.yellow, Color.green, (score - 0.5f) * 2);
        } else {
            color = Color.Lerp(Color.red, Color.yellow, score * 2);
        }

        fill.color = color;
    }
}
