using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;

public static class SaveManager
{
    public static string saveProfiles = Application.persistentDataPath + "/Profiles/";
    public static int exampleProfile = 1;

    public static PlayerData NewData(){
        int newProfileNum = getCount() + 1;
        PlayerData data = new PlayerData();
        
        SaveData(data, newProfileNum);

        return data;
    }

    public static void SaveData(PlayerData data, int profileNum){
        string jsonData = JsonUtility.ToJson(data, true);
        string filePath = saveProfiles + "GirlieData" + profileNum + ".json";
        File.WriteAllText(filePath, jsonData);
    }

    public static PlayerData LoadData(int profileNum){
        string json = File.ReadAllText(saveProfiles + "/GirlieData" + profileNum + ".json");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        GameManager.Instance.Load(profileNum, data);

        return data;
    }

    public static void DeleteData(){

    }

    private static int getCount(){
        if (Directory.Exists(saveProfiles))
        {
            DirectoryInfo d = new DirectoryInfo(saveProfiles);
            return d.GetFiles("GirlieData*.json").Length;
        }
        else
        {
            Directory.CreateDirectory(saveProfiles);
            return 0;
        }
    }
}
