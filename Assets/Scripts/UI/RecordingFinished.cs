using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nuitrack;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordingFinished : MonoBehaviour {

    public void OnRecordingFinished(IEnumerable<Skeleton> skeletonFrames) {
        VariableHolder.RecordedSkeletonFrames = skeletonFrames.Select(s => (SerializableSkeleton) s).ToList();
        SceneManager.LoadScene(SceneNames.PLAYBACK_EDIT_SCENE, LoadSceneMode.Single);
    }
}
