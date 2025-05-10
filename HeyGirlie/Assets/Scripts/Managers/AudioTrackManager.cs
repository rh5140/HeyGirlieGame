using UnityEngine;
using System.Collections.Generic;

public class AudioTrackManager : MonoBehaviour
{ 
    public AudioSource musicSource;
    public Dictionary<string, AudioClip> trackDict;

    private void Awake()
    {
        musicSource = SettingManager.Instance.music;
        VoicelineArrayContainer arr = GetComponent<VoicelineArrayContainer>();
         // Sorry this uses voiceline dictionary which I should have named more generically...
        trackDict = new Dictionary<string, AudioClip>();
        foreach (VoicelineDictionaryItem v in arr.voicelineArray)
        {
            if (!trackDict.ContainsKey(v.voicelineName))
            {
                trackDict.Add(v.voicelineName, v.voiceline);
            }
        }
    }

    public void ChangeTrack(string audioName)
    {
        musicSource.Stop();
        if (trackDict.ContainsKey(audioName)) 
        {
            musicSource.clip = trackDict[audioName];
            musicSource.Play();
        }
        else Debug.Log("Track " + audioName + " not found!");
    }
}
