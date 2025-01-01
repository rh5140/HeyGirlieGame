using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        PlayerData data = SaveManager.NewData();
        
        SceneManager.LoadScene(data.getScene());
    }

    public void LoadGame()
    {
        PlayerData data = SaveManager.LoadData(SaveManager.exampleProfile);

        SceneManager.LoadScene(data.getScene());
    }

    public void Settings()
    {
        
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
}
