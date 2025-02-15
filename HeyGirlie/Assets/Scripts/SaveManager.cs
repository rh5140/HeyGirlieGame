using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*****************************************************

Save Manager - Connects menu buttons and game manager,
as well as does the sole interactions with save files

*****************************************************/
public static class SaveManager
{
    public static string profilePath = Application.persistentDataPath + "/Profiles/";
    public static string dataPath = Application.persistentDataPath + "/Profiles/GirlieData{profileNum}.json";
    public static string screenshotPath = Application.persistentDataPath + "/Profiles/GirlieScene{profileNum}.png";
    public static int exampleProfile = 1; // This is for testing the save/load features. will be deleted once that's all done

    // Creates new save file
    public static PlayerData NewData(string playerName){
        int newProfileNum = getCount();
        Debug.Log(newProfileNum);

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
        string filePath = dataPath.Replace("{profileNum}", profileNum.ToString("00"));
        string filePath2 = screenshotPath.Replace("{profileNum}", profileNum.ToString("00"));

        ScreenCapture.CaptureScreenshot(filePath2);

        File.WriteAllText(filePath, jsonData);
    }

    // Loads in save file of specified number
    public static string LoadData(int profileNum){
        string filePath = dataPath.Replace("{profileNum}", profileNum.ToString("00"));
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
        string filePath = dataPath.Replace("{profileNum}", profileNum.ToString("00"));
        string filePath2 = screenshotPath.Replace("{profileNum}", profileNum.ToString("00"));

        File.Delete(filePath);
        File.Delete(filePath2);
        
        // for(int i = profileNum + 1; i <= count; i++){
        //     File.Move(profilePath + "GirlieData" + i + ".json", profilePath + "GirlieData" + (i-1) + ".json");
        //     File.Move(profilePath + "GirlieScene" + i + ".png", profilePath + "GirlieScene" + (i-1) + ".png");
        // }
    }

    // Counts how many profiles currently exist
    // Need to test: if a profile of an intermediate number is deleted
    public static int getCount(){
        if (Directory.Exists(profilePath))
        {
            DirectoryInfo d = new DirectoryInfo(profilePath);
            FileInfo[] files = d.GetFiles("GirlieData*.json");
            for(int i = 1; i <= 10; i++){
                // Regex.Match(files[i])
                try{
                    Debug.Log(files[i-1].Name);
                    Match m = Regex.Match(files[i-1].Name, i.ToString("00"));
                    if(!m.Success) return i;
                } catch (Exception e){
                    // Debug.Log(e.Message);
                    return i;
                }
            }
            return 11;
        }
        else
        {
            Directory.CreateDirectory(profilePath);
            return 1;
        }
    }

    public static string findSave(int profileNum){
        string filePath = dataPath.Replace("{profileNum}", profileNum.ToString("00"));

        if(File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            
            return data.getPlayerName();
        }
        else return null;
    }

    public static Sprite getScreenshot(int profileNum){
        string filePath = screenshotPath.Replace("{profileNum}", profileNum.ToString("00"));

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
