using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {

    [SerializeField] private MenuControl menuControl;

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

    public void GoToTransferScene() {
        GoToScene(SceneNames.TRANSFER_SCENE);
    }

    public void GoToOnlineScene() {
        GoToScene(SceneNames.ONLINE_SCENE);
    }

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        //StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        menuControl.OnStateChanged(MenuStates.LoadingScene, false);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    } 
}
