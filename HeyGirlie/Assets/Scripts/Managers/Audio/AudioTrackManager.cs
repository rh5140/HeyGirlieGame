using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioTrackManager : MonoBehaviour
{ 
    public AudioMixer audioMixer;
    public string currentTrack = "default";

    private void Start()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, currentTrack, 0.5f, SettingManager.Instance.music.volume));
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
