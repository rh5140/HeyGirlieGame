using UnityEngine;
using System;
using Yarn.Unity;
using TMPro;
using System.Collections.Generic;

public class SettingManager : MonoBehaviour
{
    private static SettingManager _instance;
    public static SettingManager Instance {get {return _instance;}}

    [SerializeField] public bool fullscreen = false;
    [SerializeField] public float cursor, volMusic, volSFX, volVoices, speed, textSize;
    [SerializeField] public bool autoforward = false;

    [SerializeField] public AudioSource music, sfx, voices;
    [SerializeField] private HGGLineView hggLineView = null;
    [SerializeField] private List<HGGOptionView> hggOptionViews = null;
    [SerializeField] private HGGOptionsListView hggOptionsListView = null;

    public bool fastForwardActive;

    void OnEnable() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        LoadSettings();

        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeFullscreen(bool value){
        fullscreen = value;
        if(value) Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);
        else Screen.SetResolution(3840, 2160, FullScreenMode.MaximizedWindow);
    }

    public void ChangeCursor(float value){
        cursor = value;

        if(CursorManager.Instance != null) CursorManager.Instance.ChangeCursor(value);
    }

    public void ChangeVolMusic(float value){
        volMusic = value;
        music.volume = value;
    }

    public void ChangeVolSFX(float value){
        volSFX = value;
        sfx.volume = value;
    }

    public void ChangeVolVoice(float value){
        volVoices = value;
        voices.volume = value;
    }

    public void ChangeSpeed(float value){
        speed = value;
        if(hggLineView != null) hggLineView.SetSpeed(value*100);
    }

    public void ChangeAutoforward(bool value){
        autoforward = value;
        if(hggLineView != null) hggLineView.SetAutoAdvanced(value);
    }


    public void ChangeTextSize(float value)
    {
        /* This is for if we want the settings to only have 4 text size options
        Dictionary<float, float> tempDict = new Dictionary<float, float>();
        tempDict.Add(1, 50);
        tempDict.Add(2, 60);
        tempDict.Add(3, 70);
        tempDict.Add(4, 80);
        float dictValue;
        tempDict.TryGetValue(value, out dictValue);
        */
       
        textSize = value;
        // change line view font size
        if (hggLineView != null)
        {
            hggLineView.lineText.fontSize = value;
        }
        // change last line & option view font size 
        if (hggOptionsListView != null)
        {
            // change last line font size
            hggOptionsListView.lastLineText.fontSize = value;
            // change each option view font size
            foreach (HGGOptionView option in hggOptionViews)
            {
                option.text.fontSize = value;
                //Debug.Log("changing option font size");
            }
        }
        // change option view font size
        /*
        if (hggOptionView != null)
        {
            foreach (HGGOptionView option in hggOptionViews)
            {
                option.text.fontSize = value;
            }
            // hggOptionView.text.fontSize = value;
        }
        */
        
    }
    

    public void LoadSettings(){
        if(PlayerPrefs.HasKey(nameof(Setting.Fullscreen))) fullscreen = PlayerPrefs.GetInt(nameof(Setting.Fullscreen)) != 0;
        else fullscreen = false;

        if(PlayerPrefs.HasKey(nameof(Setting.Cursor))) cursor = PlayerPrefs.GetFloat(nameof(Setting.Cursor));
        else cursor = 0f;

        if(PlayerPrefs.HasKey(nameof(Setting.Music))) volMusic = PlayerPrefs.GetFloat(nameof(Setting.Music));
        else volMusic = Settings.maxMusicVol;

        if(PlayerPrefs.HasKey(nameof(Setting.SFX))) volSFX = PlayerPrefs.GetFloat(nameof(Setting.SFX));
        else volSFX = Settings.maxSfxVol;

        if(PlayerPrefs.HasKey(nameof(Setting.Voices))) volVoices = PlayerPrefs.GetFloat(nameof(Setting.Voices));
        else volVoices = Settings.maxVoiceVol;

        if(PlayerPrefs.HasKey(nameof(Setting.Speed))) speed = PlayerPrefs.GetFloat(nameof(Setting.Speed));
        else speed = 1f;

        if(PlayerPrefs.HasKey(nameof(Setting.Autoforward))) autoforward = PlayerPrefs.GetInt(nameof(Setting.Autoforward)) != 0;
        else autoforward = false;
        
        if (PlayerPrefs.HasKey(nameof(Setting.TextSize))) textSize = PlayerPrefs.GetFloat(nameof(Setting.TextSize));
        else textSize = 68f;

        ChangeFullscreen(fullscreen);
        ChangeCursor(cursor);
        ChangeVolMusic(volMusic);
        ChangeVolSFX(volSFX);
        ChangeVolVoice(volVoices);
        ChangeSpeed(speed);
        ChangeAutoforward(autoforward);
        ChangeTextSize(textSize);
    }

    public void SetLineView(HGGLineView lineView){
        hggLineView = lineView;
    }

    /*
    public void SetOptionView(HGGOptionView optionView)
    {
        hggOptionView = optionView;
    }
    */

    public void SetOptionsListView(HGGOptionsListView optionsListView, List<HGGOptionView> optionViews)
    {
        hggOptionsListView = optionsListView;
        hggOptionViews = optionViews;
    }


    public void UpdateLineView(){
        ChangeSpeed(speed);
        ChangeAutoforward(autoforward);
        ChangeTextSize(textSize);
    }

    public void UpdateOptionView()
    {
        ChangeTextSize(textSize);
        //Debug.Log("running UpdateOptionView");
    }

}