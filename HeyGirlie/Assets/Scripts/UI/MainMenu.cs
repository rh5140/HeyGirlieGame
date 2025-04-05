using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Collections.Generic;
using TMPro;

/*****************************************************

Main Menu - Contains all main menu button
functionality

*****************************************************/
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject saveProfilesMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject newGamePopup;
    [SerializeField] private GameObject maxProfilePopup;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private AudioClip[] _startupVoicelines;

    void Start() {
        SettingManager.Instance.voices.PlayOneShot(_startupVoicelines[RandomizeVoiceline()]);
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
