using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Settings : Menu
{
    [SerializeField] private GameObject background;
    
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider cursorSlider, musicSlider, sfxSlider, voicesSlider, speedSlider, textSizeSlider; 
    [SerializeField] private Toggle autoforwardToggle;

    // these values determine the default volume when a player loads the game for the very first time; match to max slider value in inspector once music is finalized
    public static float maxMusicVol = 0.2f, maxSfxVol = 0.4f, maxVoiceVol = 1f; 

    [SerializeField] private GameObject saveGalleryMenu;

    [SerializeField] private Image screenshot;
    [SerializeField] private Sprite defaultScreenshot;

    [SerializeField] private GameObject settingsContainer;
    [SerializeField] private GameObject controlsContainer;

    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject controlsScroll;

    [SerializeField] private CanvasGroup autoforward, textSpeed;

    void Awake() {
        StartCoroutine(WaitAwake());
    }

    protected IEnumerator WaitAwake(){
        LockEsc(EscLock.Settings);
        Pause(); // Game is already paused when this is called, so nothing happens. The game unpauses when dropdown is destroyed.
        GameManager.Instance.menuOpen = true;

        if(SettingManager.Instance.fastForwardActive){
            speedSlider.interactable = false;
            autoforwardToggle.interactable = false;

            autoforward.alpha = 0.5f;
            textSpeed.alpha = 0.5f;
        }
        
        maxMusicVol = musicSlider.maxValue;
        maxSfxVol = sfxSlider.maxValue;
        maxVoiceVol = voicesSlider.maxValue;

        SetSettings();
        // Turn the settings tab/page on and the controls tab/page off
        settingsButton.GetComponent<Button>().interactable = false;
        controlsContainer.SetActive(false);

        screenshot.sprite = (SceneManager.GetActiveScene().name.Equals("Main Menu")) ? defaultScreenshot : SaveManager.getScreenshot(GameManager.Instance.GetProfile());

        yield return null;
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyStart();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Settings) Close();
    }

    void OnDestroy(){
        GameManager.Instance.menuOpen = false;
        Unpause();
        UnlockEsc();
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyEnd();
    }

    public void ToggleFullscreen(bool value){
        PlayerPrefs.SetInt(nameof(Setting.Fullscreen), value ? 1 : 0);
        SettingManager.Instance.ChangeFullscreen(value);
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
        if(!SettingManager.Instance.fastForwardActive){
            PlayerPrefs.SetFloat(nameof(Setting.Speed), value);
            SettingManager.Instance.ChangeSpeed(value);
        }
    }

    public void ToggleAutoforward(bool value){
        if(!SettingManager.Instance.fastForwardActive){
            PlayerPrefs.SetInt(nameof(Setting.Autoforward), value ? 1 : 0);
            SettingManager.Instance.ChangeAutoforward(value);
        }
    }
    
    public void ChangeTextSize(float value)
    {
        PlayerPrefs.SetFloat(nameof(Setting.TextSize), value);
        SettingManager.Instance.ChangeTextSize(value);
    }

    public void OpenSaves(){
        if(SceneManager.GetActiveScene().name.Equals("Main Menu")) Instantiate(saveGalleryMenu);
        else {
            CursorManager.Instance.WaitCursor(() => {
                Instantiate(saveGalleryMenu);
                return true;
            });
        }
    }

    private void SetSettings(){
        fullscreenToggle.isOn = SettingManager.Instance.fullscreen;
        cursorSlider.value = SettingManager.Instance.cursor;

        musicSlider.value = SettingManager.Instance.volMusic;
        sfxSlider.value = SettingManager.Instance.volSFX;
        voicesSlider.value = SettingManager.Instance.volVoices;

        speedSlider.value = SettingManager.Instance.speed;
        autoforwardToggle.isOn = SettingManager.Instance.autoforward;
        
        textSizeSlider.value = SettingManager.Instance.textSize;
    }

    //Open the settings tab/page
    public void OpenSettings()
    {
        if (settingsContainer.activeSelf == false && controlsContainer.activeSelf == true)
        {
            settingsContainer.SetActive(true);
            settingsButton.GetComponent<Button>().interactable = false;
            // Set current selected to background since explicit navigation doesn't skip past uninteractable objects
            EventSystem.current.SetSelectedGameObject(background);
            settingsContainer.GetComponent<FadeSettings>().FadeIn();
            controlsContainer.GetComponent<FadeSettings>().FadeOut();
            controlsContainer.SetActive(false);
            controlsButton.GetComponent<Button>().interactable = true;
        }
    }

    //Open the controls tab/page
    public void OpenControls()
    {
        if (settingsContainer.activeSelf == true && controlsContainer.activeSelf == false)
        {
            controlsContainer.SetActive(true);
            controlsButton.GetComponent<Button>().interactable = false;
            // Set current selected to controls scrollbar since explicit navigation doesn't skip past uninteractable objects
            EventSystem.current.SetSelectedGameObject(controlsScroll);
            controlsContainer.GetComponent<FadeSettings>().FadeIn();
            settingsContainer.GetComponent<FadeSettings>().FadeOut();
            settingsContainer.SetActive(false);
            settingsButton.GetComponent<Button>().interactable = true;
        }
    }
}

