using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackEditSetup : MonoBehaviour {

    public NativeAvatar PlaybackAvatar;
    public Playback Playback;

	void Start () {
		PlaybackAvatar.SwapSkeletonProvider(Playback);
	}
}
