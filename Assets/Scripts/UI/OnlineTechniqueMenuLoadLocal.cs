using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum UploadOptions {
    None, Technique, Attempt
}

public class OnlineTechniqueMenuLoadLocal : MonoBehaviour {

    [SerializeField] private GameObject techniqueListItemPrefab;
    [SerializeField] private VerticalLayoutGroup techniqueLayout;
    [SerializeField] private MenuControl menuControl;

    private UploadOptions uploadOption;

    public void SetupListAttempts() {
        uploadOption = UploadOptions.Technique;
        SetupList(TechniqueFileHelper.GetAllAttemptedTechniquesMeta());
    }

    public void SetupListTechniques() {
        uploadOption = UploadOptions.Attempt;
        SetupList(TechniqueFileHelper.GetCleanTechniquesMeta());
    }

    private void SetupList(IEnumerable<TechniqueMetaData> allMetaData) {
        foreach (Transform child in techniqueLayout.transform) {
            Destroy(child.gameObject);
        }

        foreach (TechniqueMetaData meta in allMetaData) {
            GameObject techniqueGameObject = Instantiate(techniqueListItemPrefab);
            techniqueGameObject.name = meta.TechniqueName;
            techniqueGameObject.transform.SetParent(techniqueLayout.transform, false);

            TextMeshProUGUI[] texts = techniqueGameObject.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts) {
                switch (text.name) {
                    case "Technique Label":
                        text.text = $"Name: {meta.TechniqueName}\n" +
                                $"Recorded by: {meta.UserName}";
                        break;

                    case "Attempt Label":
                        if (meta.HasBeenAttempted) {
                            text.text = $"Last attempted: {meta.LastAttemptedDateTime.ToShortDateString()}\n" +
                                        $"Last Score: {(int)meta.LastScorePercent + "%"}\n" +
                                        $"Best Score: {(int)meta.BestScorePercent + "%"}";
                        }
                        break;
                }
            }

            Button uploadButton = techniqueGameObject.GetComponentInChildren<Button>();
            ClickListener clickListener = uploadButton.gameObject.AddComponent<ClickListener>();

            clickListener.OnLeftClick += delegate {
                Technique loadedTechnique = TechniqueFileHelper.Load(meta.TechniqueName);
                UnityWebRequest request = null;

                switch (uploadOption) {
                    case UploadOptions.Technique:
                        request = APIHelper.CreateTechnique(loadedTechnique);
                        break;

                    case UploadOptions.Attempt:
                        request = APIHelper.SendAttempt(loadedTechnique);
                        break;
                }

            };
        }
    }
}
