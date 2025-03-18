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
        Debug.Log(string.IsNullOrEmpty(playerName.text));
        PlayerData data = (!string.IsNullOrEmpty(playerName.text)) ? SaveManager.NewData(playerName.text) : SaveManager.NewData("Kristen");
        
        if(data != null) SceneManager.LoadScene(data.getScene());
        else openMaxProfilePopup();
    }

    public void Back(){
        playerName.text = "";
        newGamePopup.SetActive(false);
        maxProfilePopup.SetActive(false);
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
    
    public void openMaxProfilePopup(){
        maxProfilePopup.SetActive(true);
    }
}
