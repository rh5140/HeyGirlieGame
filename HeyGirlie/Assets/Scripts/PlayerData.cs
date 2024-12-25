using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    [SerializeField] private string scene;
    [SerializeField] private int week;
    [SerializeField] private int datesThisWeek;

    [SerializeField] private List<LIData> liData = new List<LIData>();
    // public int points;

    [System.Serializable]
    public class LIData {
        [Range(1,8)][SerializeField] private int dateCount;
        [SerializeField] private int points;
        // [SerializeField] private int successThreshold;

        public LIData(int dateCount, int points){
            this.dateCount = dateCount;
            this.points = points;
        }
    }

    public PlayerData(string scene, int week, int datesThisWeek){
        this.scene = scene;
        this.week = week;
        this.datesThisWeek = datesThisWeek;
    }

    public void addLI(int dateCount, int points){
        liData.Add(new LIData(dateCount, points));
    }

    public string getScene(){
        return scene;
    }

    public int getWeek(){
        return week;
    }

    public int getDatesThisWeek(){
        return datesThisWeek;
    }
}
