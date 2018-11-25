using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadListSetup : MonoBehaviour {

    public GameObject ButtonPrefab;
    public MenuControl MenuControl;
    public TrainingActions TrainingActions;
    public VerticalLayoutGroup TechniqueLayout;

    void Start() {
        SetupList();
    }

    public void SetupList() {
        IEnumerable<TechniqueMetaData> metaDataCollection = TechniqueFileHelper.GetAllTechniquesMeta();

        foreach (TechniqueMetaData meta in metaDataCollection) {
            GameObject techniqueGameObject = GameObject.Find(meta.TechniqueName);
            if (techniqueGameObject == null) {
                techniqueGameObject = Instantiate(ButtonPrefab);
                techniqueGameObject.name = meta.TechniqueName;
                techniqueGameObject.transform.SetParent(TechniqueLayout.transform, false);
            }

            Button button = techniqueGameObject.GetComponent<Button>();
            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();

            button.onClick.RemoveAllListeners();

            foreach (TextMeshProUGUI text in texts) {
                if (text.name == "TechniqueDataText") {
                    text.text = $"Name: {meta.TechniqueName}\n" +
                                $"Recorded by: {meta.UserName}";
                } else if (text.name == "UserStatsText") {
                    bool shouldDisplayDefault = meta.LastAttemptedDateTime == default(DateTime);
                    string date = shouldDisplayDefault ? "N/A" : meta.LastAttemptedDateTime.ToShortDateString();
                    text.text = $"Last attempted: {date}\n" +
                                $"Last Score: {(shouldDisplayDefault ? "N/A" : (int) meta.LastScorePercent + "%")}\n" +
                                $"Best Score: {(shouldDisplayDefault ? "N/A" : (int) meta.BestScorePercent + "%")}%";
                }
            }

            button.onClick.AddListener(() => {
                Technique loadedTechnique = TechniqueFileHelper.Load(meta.TechniqueName);
                TrainingActions.Init(loadedTechnique);
                MenuControl.OnStateChanged(MenuStates.TrainingPreview);
            });
        }
    }
}
