using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;

/*****************************************************

Save Manager - Connects menu buttons and game manager,
as well as does the sole interactions with save files

*****************************************************/
public static class SaveManager
{
    public static string profilePath = Application.persistentDataPath + "/Profiles/";
    public static int exampleProfile = 1; // This is for testing the save/load features. will be deleted once that's all done

    // Creates new save file
    public static PlayerData NewData(string playerName){
        int newProfileNum = getCount() + 1;

        if(newProfileNum > 10) return null;

        PlayerData data = (playerName != null) ? new PlayerData(playerName) : new PlayerData();
        
        SaveData(data, newProfileNum);
        GameManager.Instance.SetProfile(newProfileNum);
        GameManager.Instance.SetPlayerName(playerName);

        return data;
    }

    // Saves relevant data to existing save file of specified number
    public static void SaveData(PlayerData data, int profileNum){
        string jsonData = JsonUtility.ToJson(data, true);
        string filePath = profilePath + "GirlieData" + profileNum + ".json";
        string filePath2 = profilePath + "GirlieScene" + profileNum + ".png";

        ScreenCapture.CaptureScreenshot(filePath2);

        File.WriteAllText(filePath, jsonData);
    }

    // Loads in save file of specified number
    public static string LoadData(int profileNum){
        string filePath = profilePath + "GirlieData" + profileNum + ".json";
        Debug.Log(filePath);
        
        if(findSave(profileNum) != null){
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // GameManager.Instance.Load(profileNum, data);
            GameManager.Instance.SetProfile(profileNum);
            GameManager.Instance.SetPlayerName(data.getPlayerName());
            GameManager.Instance.SetWeek(data.getWeek());
            GameManager.Instance.SetDatesThisWeek(data.getDatesThisWeek());
            
            List<int[]> lis = data.getLIs();
            GameManager.Instance.SetLiQueue(lis);

            return data.getScene();
        } else return null;
    }

    // Deletes a specified profile
    public static void DeleteData(int profileNum){
        int count = getCount();
        string filePath = profilePath + "GirlieData" + profileNum + ".json";
        string filePath2 = profilePath + "GirlieScene" + profileNum + ".png";

        File.Delete(filePath);
        File.Delete(filePath2);
        
        for(int i = profileNum + 1; i <= count; i++){
            File.Move(profilePath + "GirlieData" + i + ".json", profilePath + "GirlieData" + (i-1) + ".json");
            File.Move(profilePath + "GirlieScene" + i + ".png", profilePath + "GirlieScene" + (i-1) + ".png");
        }
    }

    // Counts how many profiles currently exist
    // Need to test: if a profile of an intermediate number is deleted
    public static int getCount(){
        if (Directory.Exists(profilePath))
        {
            DirectoryInfo d = new DirectoryInfo(profilePath);
            return d.GetFiles("GirlieData*.json").Length;
        }
        else
        {
            Directory.CreateDirectory(profilePath);
            return 0;
        }
    }

    public static string findSave(int profileNum){
        string filePath = profilePath + "GirlieData" + profileNum + ".json";

        if(File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            
            return data.getPlayerName();
        }
        else return null;
    }

    public static Sprite getScreenshot(int profileNum){
        string filePath = profilePath + "GirlieScene" + profileNum + ".png";

        if (string.IsNullOrEmpty(filePath)) return null;
        if (System.IO.File.Exists(filePath))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
}
