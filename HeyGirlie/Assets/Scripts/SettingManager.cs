using UnityEngine;
using System;
using Yarn.Unity;

public class SettingManager : MonoBehaviour
{
    private static SettingManager _instance;
    public static SettingManager Instance {get {return _instance;}}

    [SerializeField] private Texture2D appleCursor, applebeeCursor, beeCursor;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] public bool fullscreen = false, vsync = false;
    [SerializeField] public float cursor = 0f, volMusic = 1f, volSFX = 1f, volVoices = 1f, speed = 1f;
    [SerializeField] public bool autoforward = false;

    [SerializeField] public AudioSource music, sfx, voices;
    [SerializeField] private HGGLineView hggLineView = null;

    void Awake() {
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
        Screen.fullScreen = value;
    }

    public void ChangeVSync(bool value){
        vsync = value;
        Debug.Log("idk what VSync is <3");
    }

    public void ChangeCursor(float value){
        cursor = value;
        switch((int)Math.Ceiling(value)){
            case 0:
                Cursor.SetCursor(appleCursor, Vector2.zero, cursorMode);
                break;
            case 1:
                Cursor.SetCursor(applebeeCursor, Vector2.zero, cursorMode);
                break;
            case 2:
                Cursor.SetCursor(beeCursor, Vector2.zero, cursorMode);
                break;
            default:
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                break;
        }
    }

    public void ChangeVolMusic(float value){
        volMusic = value;
        music.volume = value;
    }

    public void ChangeVolSFX(float value){
        volSFX = value;
        sfx.volume = value;
    }

    public void ChangeVolVoices(float value){
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

    public void LoadSettings(){
        if(PlayerPrefs.HasKey(nameof(Setting.Fullscreen))) fullscreen = PlayerPrefs.GetInt(nameof(Setting.Fullscreen)) != 0;
        else fullscreen = false;

        if(PlayerPrefs.HasKey(nameof(Setting.VSync))) vsync = PlayerPrefs.GetInt(nameof(Setting.VSync)) != 0;
        else vsync = false;

        if(PlayerPrefs.HasKey(nameof(Setting.Cursor))) cursor = PlayerPrefs.GetFloat(nameof(Setting.Cursor));
        else cursor = 0f;

        if(PlayerPrefs.HasKey(nameof(Setting.Music))) volMusic = PlayerPrefs.GetFloat(nameof(Setting.Music));
        else volMusic = 1f;

        if(PlayerPrefs.HasKey(nameof(Setting.SFX))) volSFX = PlayerPrefs.GetFloat(nameof(Setting.SFX));
        else volSFX = 1f;

        if(PlayerPrefs.HasKey(nameof(Setting.Voices))) volVoices = PlayerPrefs.GetFloat(nameof(Setting.Voices));
        else volVoices = 1f;

        if(PlayerPrefs.HasKey(nameof(Setting.Speed))) speed = PlayerPrefs.GetFloat(nameof(Setting.Speed));
        else speed = 1f;

        if(PlayerPrefs.HasKey(nameof(Setting.Autoforward))) autoforward = PlayerPrefs.GetInt(nameof(Setting.Autoforward)) != 0;
        else autoforward = false;

        ChangeFullscreen(fullscreen);
        ChangeVSync(vsync);
        ChangeCursor(cursor);
        ChangeVolMusic(volMusic);
        ChangeVolSFX(volSFX);
        ChangeVolVoices(volVoices);
        ChangeSpeed(speed);
        ChangeAutoforward(autoforward);
    }

    public void SetLineView(HGGLineView lineView){
        hggLineView = lineView;
    }
}