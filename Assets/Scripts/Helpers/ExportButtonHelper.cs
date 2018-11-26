using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExportButtonHelper : MonoBehaviour {

    public InputField TechniqueNameField, UserNameField;
    public Playback Playback;

    [SerializeField] private TextMeshProUGUI exportedText;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private MenuControl menuControl;

    public void Export() {
        errorText.text = String.Empty;

        string techniqueName = TechniqueNameField.text;
        string userName = UserNameField.text;

        bool validTechName = ValidTechName();
        bool validUserName = ValidUserName();
        bool validData = validUserName && validTechName;

        if (!validUserName) {
            errorText.text += "Invalid username.\n";
        }

        if (!validTechName) {
            errorText.text += "Invalid technique name";
        }

        if (validData) {
            SerializableSkeleton[] techniqueFrames = Playback.SkeletonFrames
                .Where((s, i) => i >= Playback.StartFrame && i < Playback.EndFrame)
                .ToArray();

            TechniqueFileHelper.Save(new Technique(techniqueName, techniqueFrames, userName));

            exportedText.text = $"{techniqueName} exported successfully.";
            menuControl.OnStateChanged(MenuStates.ExportFinished);
        } else {
            menuControl.OnStateChanged(MenuStates.ErrorEdit, false);
        }
    }

    private bool ValidTechName() {
        return !String.IsNullOrWhiteSpace(TechniqueNameField.text);
    }

    private bool ValidUserName() {
        return !String.IsNullOrWhiteSpace(UserNameField.text);
    }
}
