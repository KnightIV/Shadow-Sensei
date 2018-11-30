using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadListSetup : MonoBehaviour {

    public const string DELETE = "Are you sure you want to delete {0}?";

    public GameObject ButtonPrefab;
    public MenuControl MenuControl;
    public TrainingActions TrainingActions;
    public VerticalLayoutGroup TechniqueLayout;

    [SerializeField] private TextMeshProUGUI promptToDelete;

    void Start() {
        SetupList();
    }

    public void SetupList() {
        IEnumerable<TechniqueMetaData> metaDataCollection = TechniqueFileHelper.GetAllTechniquesMeta();

        foreach (Transform child in TechniqueLayout.transform) {
            if (!metaDataCollection.Any(t => t.TechniqueName == child.name)) {
                Destroy(child.gameObject);
            }
        }

        foreach (TechniqueMetaData meta in metaDataCollection) {
            GameObject techniqueGameObject = GameObject.Find(meta.TechniqueName);
            if (techniqueGameObject == null) {
                techniqueGameObject = Instantiate(ButtonPrefab);
                techniqueGameObject.name = meta.TechniqueName;
                techniqueGameObject.transform.SetParent(TechniqueLayout.transform, false);
            }

            Button button = techniqueGameObject.GetComponent<Button>();
            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI text in texts) {
                if (text.name == "TechniqueDataText") {
                    text.text = $"Name: {meta.TechniqueName}\n" +
                                $"Recorded by: {meta.UserName}";
                } else if (text.name == "UserStatsText") {
                    bool shouldDisplayDefault = meta.LastAttemptedDateTime == default(DateTime);
                    string date = shouldDisplayDefault ? "N/A" : meta.LastAttemptedDateTime.ToShortDateString();
                    text.text = $"Last attempted: {date}\n" +
                                $"Last Score: {(shouldDisplayDefault ? "N/A" : (int) meta.LastScorePercent + "%")}\n" +
                                $"Best Score: {(shouldDisplayDefault ? "N/A" : (int) meta.BestScorePercent + "%")}";
                }
            }

            if (techniqueGameObject.GetComponent<ClickListener>() == null) {
                ClickListener clickListener = techniqueGameObject.AddComponent<ClickListener>();

                clickListener.OnRightClick += delegate {
                    promptToDelete.text = String.Format(DELETE, meta.TechniqueName);
                    VariableHolder.TechniqueToDelete = meta.TechniqueName;

                    MenuControl.OnStateChanged(MenuStates.DeleteTechnique, false);
                };
                clickListener.OnLeftClick += delegate {
                    Technique loadedTechnique = TechniqueFileHelper.Load(meta.TechniqueName);
                    TrainingActions.Init(loadedTechnique);
                    MenuControl.OnStateChanged(MenuStates.TrainingPreview);
                };
            }
        }
    }
}
