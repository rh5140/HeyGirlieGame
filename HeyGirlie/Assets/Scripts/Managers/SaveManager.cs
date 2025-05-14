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
    // Creates new save file
    public static PlayerData NewData(string playerName){
        int newProfileNum = getCount();

        if(newProfileNum > 10) return null;

        PlayerData data = (playerName != null) ? new PlayerData(playerName) : new PlayerData();
        
        SaveData(data, newProfileNum);
        GameManager.Instance.SaveProfile = newProfileNum;
        GameManager.Instance.PlayerName = playerName;

        return data;
    }

    // Saves relevant data to existing save file of specified number
    public static void SaveData(PlayerData data, int profileNum){
        string jsonData = JsonUtility.ToJson(data, true);
        string filePath = getFile(profileNum, "json");
        string filePath2 = getFile(profileNum, "png");
        // string filePath2 = screenshotPath.Replace("{profileNum}", profileNum.ToString("00"));

        // ScreenCapture.CaptureScreenshot(filePath2);
        if(ScreenshotCam.Instance != null) ScreenshotCam.Instance.Screenshot(filePath2);

        File.WriteAllText(filePath, jsonData);
    }

    // Loads in save file of specified number
    public static string LoadData(int profileNum){
        string filePath = getFile(profileNum, "json");
        
        if(findSave(profileNum) != null){
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            GameManager.Instance.SaveProfile = profileNum;
            GameManager.Instance.PlayerName = data.PlayerName;
            // GameManager.Instance.Location(data.Location);
            GameManager.Instance.Week = data.Week;
            GameManager.Instance.DatesThisWeek = data.DatesThisWeek;

            GameManager.Instance.Priority = (Character) data.Priority;
            GameManager.Instance.PolyamPartner = (Character) data.PolyamPartner;
            GameManager.Instance.PolyamActive = (data.PolyamActive == 1) ? true : false;
            GameManager.Instance.PolyamPair = (Character) data.PolyamPair;
            
            GameManager.Instance.SetLiQueue(data.getLIs());
            GameManager.Instance.SetAyda(data.Ayda);
            GameManager.Instance.SetFigW4(data.FigW4);

            GameManager.Instance.SetLocationQueues(data.getLocationQueue(Region.Away), data.getLocationQueue(Region.Outdoors),
                                                    data.getLocationQueue(Region.School), data.getLocationQueue(Region.Mordred), 
                                                    data.getLocationQueue(Region.Elmville));

            return data.Scene;
        } else return null;
    }

    // Deletes a specified profile
    public static void DeleteData(int profileNum){
        string filePath = getFile(profileNum, "json");
        string filePath2 = getFile(profileNum, "png");

        File.Delete(filePath);
        File.Delete(filePath2);
    }

    // Counts how many profiles currently exist
    public static int getCount(){
        string profilePath = "";
        #if (UNITY_WEBGL)
            profilePath = "/idbfs/HGHQ/HeyGirlie/Profiles/";
        #else
            profilePath = Application.persistentDataPath + "/Profiles/";
        #endif

        if (Directory.Exists(profilePath))
        {
            DirectoryInfo d = new DirectoryInfo(profilePath);
            FileInfo[] files = d.GetFiles("Girlie*.json");
            for(int i = 1; i <= 10; i++){
                // Regex.Match(files[i])
                try{
                    // Debug.Log(files[i-1].Name);
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

    public static PlayerData findSave(int profileNum){
        string filePath = getFile(profileNum, "json");

        if(File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            
            return data;
        }
        else return null;
    }

    public static Sprite getScreenshot(int profileNum){
        string filePath = getFile(profileNum, "png");

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

    private static string getFile(int num, string extension){
        string path = "/Profiles/Girlie{0}.{1}";
        
        #if (UNITY_WEBGL)
            return String.Format("/idbfs/HGHQ/HeyGirlie/" + path, num.ToString("00"), extension);
        #else
            return String.Format(Application.persistentDataPath + path, num.ToString("00"), extension);
        #endif
    }
}
