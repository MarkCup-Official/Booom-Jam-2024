using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip musicClip;
    public void ReplaceCurrentMusic(bool isLoop)
    {
        MusicManager.Instance.Play(musicClip, isLoop);
        
    }
}
