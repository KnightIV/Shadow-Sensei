using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackEditSetup : MonoBehaviour {

    public NativeAvatar PlaybackAvatar;
    public RiggedAvatar RiggedPlaybackAvatar;
    public Playback Playback;

	void Start () {
        IAvatar playbackAvatar = RiggedPlaybackAvatar as IAvatar ?? PlaybackAvatar;
        playbackAvatar.SwapSkeletonProvider(Playback);
	}
}
