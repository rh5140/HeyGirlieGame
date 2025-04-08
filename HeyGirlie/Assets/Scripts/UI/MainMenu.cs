using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

/*****************************************************

Main Menu - Contains all main menu button
functionality

*****************************************************/
public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menu;
    [SerializeField] private GameObject stickerAnimation;
    [SerializeField] private GameObject titleSticker;
    [SerializeField] private Image stickerShadow;
    
    [SerializeField] private GameObject saveProfilesMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject newGamePopup;
    [SerializeField] private GameObject maxProfilePopup;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private AudioClip[] _startupVoicelines;

    void Start() {
        StartCoroutine(AnimateMenu());
    }

    IEnumerator AnimateMenu(){
        float time = 0, lerpTime = 0.5f;
        while(time < lerpTime){
            menu.anchoredPosition = new Vector2(0, Mathf.Lerp(-2160, 0, time / lerpTime));
            time += Time.deltaTime;

            yield return null;
        }

        time = 0; lerpTime = 0.25f;
        yield return new WaitForSeconds(lerpTime);
        stickerAnimation.SetActive(true);
        
        while(time < lerpTime){
            Color c = stickerShadow.color;
            c.a = Mathf.Lerp(0f, 0.25f, time / lerpTime);
            stickerShadow.color = c;
            time += Time.deltaTime;

            yield return null;
        }

        PlayStartup();

        RectTransform stickerTransform = titleSticker.GetComponent<RectTransform>();
        Image stickerImage = titleSticker.GetComponent<Image>();
        time = 0; lerpTime = 0.25f;
        while(time < lerpTime){
            Color c = stickerShadow.color;
            c.a = Mathf.Lerp(0.25f, 0.5f, time / lerpTime);
            stickerShadow.color = c;
            
            c = stickerImage.color;
            c.a = Mathf.Lerp(0, 1f, time / lerpTime)*2f;
            stickerImage.color = c;

            stickerTransform.localScale = Mathf.Lerp(5, 1, time / lerpTime)*Vector3.one;
            time += Time.deltaTime;

            yield return null;
        }

        stickerTransform.localScale = Vector3.one;
    }

    private void Update(){
        // Debug.Log(EventSystem.current.currentSelectedGameObject.transform.name);
        if (Input.GetKeyUp(KeyCode.Return) &&
        EventSystem.current.currentSelectedGameObject.transform.name == "Input") Continue();
    }

    public void NewGame()
    {
        newGamePopup.SetActive(true);
    }

    public void Continue(){
        PlayerData data = (!string.IsNullOrEmpty(playerName.text)) ? SaveManager.NewData(playerName.text) : SaveManager.NewData("Kristen");
        
        if(data != null) SceneManager.LoadScene(data.getScene());
        else maxProfilePopup.SetActive(true);
    }

    public void Back(){
        playerName.text = "";
        newGamePopup.SetActive(false);
    }

    public void LoadGame()
    {
       Instantiate(saveProfilesMenu);
    }

    public void Settings()
    {
        Instantiate(settingsMenu);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void PlayStartup(){
        SettingManager.Instance.voices.Stop();
        SettingManager.Instance.voices.PlayOneShot(_startupVoicelines[RandomizeVoiceline()]);
    }

    private int RandomizeVoiceline()
    {
        // Weighted randomization... equal chance for everyone EXCEPT the weird K2 one
        int rand = UnityEngine.Random.Range(0, 82);
        if (rand < 10) return (int)Character.Kristen;
        else if (rand < 20) return (int)Character.Cassandra;
        else if (rand < 30) return (int)Character.Fig;
        else if (rand < 40) return (int)Character.Gertie;
        else if (rand < 50) return (int)Character.Kipperlilly;
        else if (rand < 60) return (int)Character.Lucy;
        else if (rand < 70) return (int)Character.Naradriel;
        else if (rand < 80) return (int)Character.Tracker;
        else return 8; // 8 corresponds with K2? voiceline
    }
}
