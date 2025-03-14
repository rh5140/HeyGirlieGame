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
    [SerializeField] private Slider cursorSlider, musicSlider, sfxSlider, voicesSlider, speedSlider;
    [SerializeField] private Toggle autoforwardToggle;

    [SerializeField] private Texture2D appleCursor, applebeeCursor, beeCursor;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject saveProfilesMenu;

    [SerializeField] private Image screenshot;
    [SerializeField] private Sprite defaultScreenshot;

    void Awake() {
        LoadSettings();

        screenshot.sprite = (GameManager.Instance.GetProfile() != 0) ? SaveManager.getScreenshot(GameManager.Instance.GetProfile()) : defaultScreenshot;
    }

    public void ToggleFullscreen(bool value){
        PlayerPrefs.SetInt(nameof(Setting.Fullscreen), value ? 1 : 0);
    }

    public void ToggleVSync(bool value){
        PlayerPrefs.SetInt(nameof(Setting.VSync), value ? 1 : 0);
    }

    public void ChangeCursor(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Cursor), value);

        Debug.Log("cursor");
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
        Debug.Log("music");
        PlayerPrefs.SetFloat(nameof(Setting.Music), value);
    }

    public void ChangeVolSFX(float value){
        Debug.Log("sfx");
        PlayerPrefs.SetFloat(nameof(Setting.SFX), value);
    }

    public void ChangeVolVoice(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Voices), value);
    }

    public void ChangeSpeed(float value){
        PlayerPrefs.SetFloat(nameof(Setting.Speed), value);
    }

    public void ToggleAutoforward(bool value){
        PlayerPrefs.SetInt(nameof(Setting.Autoforward), value ? 1 : 0);
    }

    public void OpenSaves(){
        Instantiate(saveProfilesMenu);
    }

    public void Close(){
        Destroy(settingsMenu);
    }

    private void LoadSettings(){
        if(PlayerPrefs.HasKey(nameof(Setting.Fullscreen))) fullscreenToggle.isOn = PlayerPrefs.GetInt(nameof(Setting.Fullscreen)) != 0;
        else fullscreenToggle.isOn = false;

        if(PlayerPrefs.HasKey(nameof(Setting.VSync))) vsyncToggle.isOn = PlayerPrefs.GetInt(nameof(Setting.VSync)) != 0;
        else vsyncToggle.isOn = false;

        if(PlayerPrefs.HasKey(nameof(Setting.Cursor))) cursorSlider.value = PlayerPrefs.GetFloat(nameof(Setting.Cursor));
        else cursorSlider.value = 0f;

        if(PlayerPrefs.HasKey(nameof(Setting.Music))) musicSlider.value = PlayerPrefs.GetFloat(nameof(Setting.Music));
        else musicSlider.value = 0.5f;

        if(PlayerPrefs.HasKey(nameof(Setting.SFX))) sfxSlider.value = PlayerPrefs.GetFloat(nameof(Setting.SFX));
        else sfxSlider.value = 0.5f;

        if(PlayerPrefs.HasKey(nameof(Setting.Voices))) voicesSlider.value = PlayerPrefs.GetFloat(nameof(Setting.Voices));
        else voicesSlider.value = 0.5f;

        if(PlayerPrefs.HasKey(nameof(Setting.Speed))) speedSlider.value = PlayerPrefs.GetFloat(nameof(Setting.Speed));
        else speedSlider.value = 1f;

        if(PlayerPrefs.HasKey(nameof(Setting.Autoforward))) autoforwardToggle.isOn = PlayerPrefs.GetInt(nameof(Setting.Autoforward)) != 0;
        else autoforwardToggle.isOn = false;
    }
}

