using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExportButtonHelper : MonoBehaviour {

    public InputField TechniqueNameField, UserNameField;
    public Playback Playback;

    public void Export() {
        SerializableSkeleton[] techniqueFrames = Playback.SkeletonFrames
            .Where((s, i) => i >= Playback.StartFrame && i < Playback.EndFrame)
            .ToArray();

        TechniqueFileHelper.Save(new Technique(TechniqueNameField.text, techniqueFrames, UserNameField.text));
    }
}
