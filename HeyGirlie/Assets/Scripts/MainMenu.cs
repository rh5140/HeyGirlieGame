using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/GirlieData.json");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        SceneManager.LoadScene(data.getScene());
    }
}
