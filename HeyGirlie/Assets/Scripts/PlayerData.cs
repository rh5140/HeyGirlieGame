using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

/*****************************************************

Player Data - Creates a serializable object that
contains all relevant information for a player save.
(Needs to be serializable for JSONUtility to work)

*****************************************************/
[Serializable]
public class PlayerData
{
    [SerializeField] private string scene;
    [SerializeField] private int week;
    [SerializeField] private int datesThisWeek;

    [SerializeField] private List<LIData> liData = new List<LIData>();
 
    // Object for LI data without making a whole new file. Not relevant outside this scope anyways
    [System.Serializable]
    public class LIData {
        [SerializeField] private int priority;
        [Range(1,8)][SerializeField] private int dateCount;
        [SerializeField] private int points;
        // [SerializeField] private int successThreshold;

        public LIData(int priority, int dateCount, int points){
            this.priority = priority;
            this.dateCount = dateCount;
            this.points = points;
        }

        public int getPriority(){
            return priority;
        }

        public int getDateCount(){
            return dateCount;
        }

        public int getPoints(){
            return points;
        }
    }

    public PlayerData(){
        this.scene = "Intro";
        this.week = 1;
        this.datesThisWeek = 0;

        for(int i = 0; i < 8; i++){
            addLI(i, 1, 0);
        }
    }

    public PlayerData(string scene, int week, int datesThisWeek){
        this.scene = scene;
        this.week = week;
        this.datesThisWeek = datesThisWeek;
    }

    // Saves individual LI data to list
    public void addLI(int priority, int dateCount, int points){
        liData.Add(new LIData(priority, dateCount, points));
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

    // Pulls specified LI's data
    public int[] GetLoveInterest(int index){
        LIData liData = this.liData[index];
        int[] li = {liData.getPriority(), liData.getDateCount(), liData.getPoints()};
        
        return li;
    }
}
