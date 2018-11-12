using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonClickHelper : MonoBehaviour {

    public Playback Playback;
    public NativeAvatar SkeletonAvatar;
    
    public void TogglePlay() {
        Playback.TogglePlay();

        SkeletonAvatar.SwapSkeletonProvider(Playback);
    }
}
