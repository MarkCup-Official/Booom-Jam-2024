using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using GameFramework.UI;
public class SettingPannel : BasePannel
{
    public Slider auidoSlider;
    public Slider musicSlider;
    public AudioMixer audioMixer;
    public override void OnClose()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }
    public override void OnShowUp()
    {
       gameObject.SetActive(true);
    }
    public void Apply()
    {
        int clamped = (int)(20 * Mathf.Log10(Mathf.Clamp(auidoSlider.value, 0.0001f, 1)));
        int clamped1 = (int)(20 * Mathf.Log10(Mathf.Clamp(musicSlider.value, 0.0001f, 1)));

        audioMixer.SetFloat("AudioVolume", clamped);
        audioMixer.SetFloat("MusicVolume", clamped1);
    }
    public void Back()
    {
        OnClose();
    }
    public void QUITGAME()
    {
        Application.Quit();
    }
    public override void OnUpdate()
    {
        
    }
}
