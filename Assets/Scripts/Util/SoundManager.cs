using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager : BaseMonoManager<SoundManager>
{
    private AudioSource m_AudioSource;
    public Dictionary<string, AudioClip> clipDic = new();
    public Dictionary<string, float> intervalDic = new();
    protected override void Awake()
    {
        base.Awake();
        m_AudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

  
    public void PlaySoundWithInterval(string groupID,string soundName, float intervalTime, float volume = 1)
    {
        if (!intervalDic.ContainsKey(groupID)) intervalDic.Add(groupID, -intervalTime);
        if (Time.time > intervalTime + intervalDic[groupID])
        {
            intervalDic[groupID] = Time.time;
            soundName = "Sounds/" + soundName;
            if (!clipDic.ContainsKey(soundName))
            {
                AudioClip clip = Resources.Load<AudioClip>(soundName);
                clipDic.Add(soundName, clip);
            }
            m_AudioSource.PlayOneShot(clipDic[soundName], volume);
        }


    }  
    public void PlaySound(string soundName, float volume = 1)
    {
        soundName = "Sounds/" + soundName;
        if (!clipDic.ContainsKey(soundName))
        {
            AudioClip clip = Resources.Load<AudioClip>(soundName);
            clipDic.Add(soundName, clip);
        }
        PlaySound(clipDic[soundName], volume);
    }
    public void PlaySoundRand(List<string> soundNameList,float volume = 1)
    {
        int index = Random.Range(0, soundNameList.Count);
        PlaySound(soundNameList[index], volume);
    }
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        m_AudioSource.PlayOneShot(clip, volume);
    }
    public AudioClip GetClip(string soundName)
    {
        if (!clipDic.ContainsKey(soundName))
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
            clipDic.Add(soundName, clip);
        }
        return clipDic[soundName];
    }
}
