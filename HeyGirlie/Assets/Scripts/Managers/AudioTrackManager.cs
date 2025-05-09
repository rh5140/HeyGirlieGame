using UnityEngine;
using System.Collections.Generic;

public class AudioTrackManager : MonoBehaviour
{
    private static AudioTrackManager _instance;
    public static AudioTrackManager Instance {get {return _instance;}}
    
    public AudioSource musicSource;
    public Dictionary<string, AudioClip> trackDict;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        VoicelineArrayContainer arr = GetComponent<VoicelineArrayContainer>();
         // Sorry this uses voiceline dictionary which I should have named more generically...
        trackDict = new Dictionary<string, AudioClip>();
        foreach (VoicelineDictionaryItem v in arr.voicelineArray)
        {
            trackDict.Add(v.voicelineName, v.voiceline);
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
