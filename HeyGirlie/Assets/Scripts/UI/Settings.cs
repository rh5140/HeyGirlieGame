using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle, vsyncToggle;
    [SerializeField] private Slider cursorSlider, musicSlider, sfxSlider, voicesSlider, speedSlider, textSizeSlider; 
    [SerializeField] private Toggle autoforwardToggle;

    public static float maxMusicVol, maxSfxVol, maxVoiceVol;

    [SerializeField] private GameObject saveProfilesMenu;

    [SerializeField] private Image screenshot;
    [SerializeField] private Sprite defaultScreenshot;

    private bool pauseLock = false;

    void Awake() {
        if(!GameManager.Instance.pauseLock){
            GameManager.Instance.Pause(true);
            pauseLock = true;
        }

        maxMusicVol = musicSlider.maxValue;
        maxSfxVol = sfxSlider.maxValue;
        maxVoiceVol = voicesSlider.maxValue;

        SetSettings();

        screenshot.sprite = (GameManager.Instance.GetProfile() != 0) ? SaveManager.getScreenshot(GameManager.Instance.GetProfile()) : defaultScreenshot;
    }

    void OnDestroy(){
        if(pauseLock){
            GameManager.Instance.Pause(false);
            pauseLock = false;
        }
    }

    public void ToggleFullscreen(bool value){
        PlayerPrefs.SetInt(nameof(Setting.Fullscreen), value ? 1 : 0);
        SettingManager.Instance.ChangeFullscreen(value);
    }

    public void ToggleVSync(bool value){
        PlayerPrefs.SetInt(nameof(Setting.VSync), value ? 1 : 0);
        SettingManager.Instance.ChangeVSync(value);
    }

    public void ChangeCursor(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Cursor), value);
        SettingManager.Instance.ChangeCursor(value);
    }

    public void ChangeVolMusic(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Music), value);
        SettingManager.Instance.ChangeVolMusic(value);
    }

    public void ChangeVolSFX(float value){
        PlayerPrefs.SetFloat(nameof(Setting.SFX), value);
        SettingManager.Instance.ChangeVolSFX(value);
    }

    public void ChangeVolVoice(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Voices), value);
        SettingManager.Instance.ChangeVolVoice(value);
    }

    public void ChangeSpeed(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Speed), value);
        SettingManager.Instance.ChangeSpeed(value);
    }

    public void ToggleAutoforward(bool value){
        PlayerPrefs.SetInt(nameof(Setting.Autoforward), value ? 1 : 0);
        SettingManager.Instance.ChangeAutoforward(value);
    }
    
    public void ChangeTextSize(float value)
    {
        PlayerPrefs.SetFloat(nameof(Setting.TextSize), value);
        SettingManager.Instance.ChangeTextSize(value);
    }

    public void OpenSaves(){
        Instantiate(saveProfilesMenu);
    }

    public void Close(){
        Destroy(gameObject);
    }

    private void SetSettings(){
        fullscreenToggle.isOn = SettingManager.Instance.fullscreen;
        vsyncToggle.isOn = SettingManager.Instance.vsync;
        cursorSlider.value = SettingManager.Instance.cursor;

        musicSlider.value = SettingManager.Instance.volMusic;
        sfxSlider.value = SettingManager.Instance.volSFX;
        voicesSlider.value = SettingManager.Instance.volVoices;

        speedSlider.value = SettingManager.Instance.speed;
        autoforwardToggle.isOn = SettingManager.Instance.autoforward;
        textSizeSlider.value = SettingManager.Instance.textSize;
    }
}

