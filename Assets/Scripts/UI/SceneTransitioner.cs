using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {

    public void GoToMainMenuScene() {
        GoToScene(SceneNames.MAIN_MENU_SCENE);
    }

    public void GoToPlaybackScene() {
        GoToScene(SceneNames.PLAYBACK_EDIT_SCENE);
    }

    public void GoToRecordScene() {
        GoToScene(SceneNames.RECORD_SCENE);
    }

    public void GoToTrainingScene() {
        GoToScene(SceneNames.TRAINING_SCENE);
    }

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
