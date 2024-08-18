using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : BaseMonoManager<MusicManager>
{
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
    public void Play(AudioClip clip,bool isloop)
    {
        audioSource.clip = clip;
        audioSource.loop = isloop;
        audioSource.Play();
    }
}
