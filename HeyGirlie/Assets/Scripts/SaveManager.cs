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
    public static string saveProfiles = Application.persistentDataPath + "/Profiles/";
    public static int exampleProfile = 1; // This is for testing the save/load features. will be deleted once that's all done

    // Creates new save file
    public static PlayerData NewData(){
        int newProfileNum = getCount() + 1;
        PlayerData data = new PlayerData();
        
        SaveData(data, newProfileNum);

        return data;
    }

    // Saves relevant data to existing save file of specified number
    public static void SaveData(PlayerData data, int profileNum){
        string jsonData = JsonUtility.ToJson(data, true);
        string filePath = saveProfiles + "GirlieData" + profileNum + ".json";
        File.WriteAllText(filePath, jsonData);
    }

    // Loads in save file of specified number
    public static PlayerData LoadData(int profileNum){
        string json = File.ReadAllText(saveProfiles + "/GirlieData" + profileNum + ".json");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        GameManager.Instance.Load(profileNum, data);

        return data;
    }

    // Deletes a specified profile
    public static void DeleteData(){

    }

    // Counts how many profiles currently exist
    // Questions: - Is there a profile max? pls say yes
    // Need to test: if a profile of an intermediate number is deleted
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
