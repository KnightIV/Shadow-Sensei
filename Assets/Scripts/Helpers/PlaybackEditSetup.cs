using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackEditSetup : MonoBehaviour {

    public NativeAvatar PlaybackAvatar;
    public RiggedAvatar RiggedPlaybackAvatar;
    public Playback Playback;
    [SerializeField] private InputField usernameInput;

	void Start () {
        IAvatar playbackAvatar = RiggedPlaybackAvatar as IAvatar ?? PlaybackAvatar;
        playbackAvatar.SwapSkeletonProvider(Playback);
        Playback.Reset();
	}

    public void EditTextInput() {
        if (VariableHolder.User.IsLoggedIn()) {
            usernameInput.text = VariableHolder.User.Username;
            usernameInput.interactable = false;
        }
    }
}
