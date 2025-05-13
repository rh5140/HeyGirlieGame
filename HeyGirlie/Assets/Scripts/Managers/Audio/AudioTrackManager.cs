using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioTrackManager : MonoBehaviour
{ 
    public AudioMixer audioMixer;
    public string currentTrack = "default";
    public bool fadeInOnStart = false;

    private void Start()
    {
        if (fadeInOnStart) 
            StartCoroutine(FadeMixerGroup.StartFade(audioMixer, currentTrack, 0.5f, SettingManager.Instance.music.volume));
    }

    public void UpdateVolume(float val)
    {
        audioMixer.SetFloat(currentTrack, Mathf.Log10(val) * 20);
    }

    public void ChangeTrack(string audioName)
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, currentTrack, 0.5f, 0));
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, audioName, 0.5f, SettingManager.Instance.music.volume));
        currentTrack = audioName;
    }
    
    public void FadeOutTrack()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, currentTrack, 0.5f, 0));
    }
}
