using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    [SerializeField] private GameObject saveGalleryMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject newGamePopup;
    [SerializeField] private GameObject maxProfilePopup;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private AudioClip[] _startupVoicelines;

    void Start() {
        StartCoroutine(AnimateMenu());
    }

    IEnumerator AnimateMenu(){
        yield return new WaitForSeconds(1);

        float start = Mathf.Ceil(((gameObject.GetComponent<RectTransform>().rect.height/2) + 1080)*1.1f);
        float time = 0, lerpTime = 0.5f;
        while(time < lerpTime){
            menu.anchoredPosition = new Vector2(0, Mathf.Lerp(-1*start, 0, time / lerpTime));
            time += Time.deltaTime;

            yield return null;
        }
        menu.anchoredPosition = Vector2.zero;

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

        yield return null;
        gameObject.GetComponent<ArrowNavigation>().EnableEventSystem();
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
        CursorManager.Instance.WaitCursor(() => {
            Instantiate(saveGalleryMenu);
            return true;
        });
    }

    public void Settings()
    {
        Instantiate(settingsMenu);
    }

    public void Credits()
    {
        CursorManager.Instance.Load(true);
        // SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_WEBGL)
            SceneManager.LoadScene("Main Menu");
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
