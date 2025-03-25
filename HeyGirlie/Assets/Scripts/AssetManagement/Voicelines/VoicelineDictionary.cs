using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class VoicelineDictionary : MonoBehaviour
{
    [SerializeField] public VoicelineArrayContainer[] voicelineArrays;
    public Dictionary<string, AudioClip> voicelineDict;

    public void Awake()
    {
        voicelineDict = new Dictionary<string, AudioClip>();
        foreach (VoicelineArrayContainer arr in voicelineArrays)
        {
            foreach (VoicelineDictionaryItem v in arr.voicelineArray)
            {
                voicelineDict.Add(v.voicelineName, v.voiceline);
            }
        }
    }
}
