using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

/*****************************************************

Player Data - Creates a serializable object that
contains all relevant information for a player save.
(Needs to be serializable for JSONUtility to work)

*****************************************************/
[Serializable]
public class PlayerData
{
    // private int saveProfile;
    [SerializeField] private string playerName;
    [SerializeField] private string scene;
    [SerializeField] private int week;
    [SerializeField] private int datesThisWeek;

    [SerializeField] private List<LIData> liData = new List<LIData>();
    
    [SerializeField] private List<string> schoolDates;
    [SerializeField] private List<string> elmvilleDates;
    [SerializeField] private List<string> mordredDates;
    [SerializeField] private List<string> outdoorsDates;
    [SerializeField] private List<string> awayDates;
 
    // Object for LI data without making a whole new file. Not relevant outside this scope anyways
    [System.Serializable]
    public class LIData {
        [SerializeField] private Character character;
        [Range(1,8)][SerializeField] private int dateCount;
        [SerializeField] private int points;
        // [SerializeField] private int successThreshold;

        public LIData(Character character, int dateCount, int points){
            this.character = character;
            this.dateCount = dateCount;
            this.points = points;
        }

        public Character getCharacter(){
            return character;
        }

        public int getDateCount(){
            return dateCount;
        }

        public int getPoints(){
            return points;
        }
    }

    public PlayerData() : this("Kristen") {}

    public PlayerData(string playerName) : this(playerName, "Intro", 1, 0, null, null, null, null, null, null) {
        for(int i = 2; i < 8; i++){
            addLI((Character)i, 1, 0);
        }
    }

    // public PlayerData(string playerName, string scene, int week, int datesThisWeek, List<LoveInterest> liQueue){
    //     this.playerName = playerName;
    //     this.scene = scene;
    //     this.week = week;
    //     this.datesThisWeek = datesThisWeek;
         
    //     if(liQueue != null){
    //         foreach(LoveInterest li in liQueue){
    //             if(li != null) addLI(li.GetCharacter(), li.GetDateCount(), li.GetPoints());
    //         }
    //     }
    // }

    public PlayerData(string playerName, string scene, int week, int datesThisWeek, List<LoveInterest> liQueue,
                        Queue<string> schoolDates, Queue<string> elmvilleDates, Queue<string> mordredDates,
                        Queue<string> outdoorsDates, Queue<string> awayDates){
        this.playerName = playerName;
        this.scene = scene;
        this.week = week;
        this.datesThisWeek = datesThisWeek;
         
        if(liQueue != null){
            foreach(LoveInterest li in liQueue){
                if(li != null) addLI(li.GetCharacter(), li.GetDateCount(), li.GetPoints());
            }
        }

        this.schoolDates = (schoolDates != null) ? schoolDates.ToList() : null;
        this.elmvilleDates = (elmvilleDates != null) ? elmvilleDates.ToList() : null;
        this.mordredDates = (mordredDates != null) ? mordredDates.ToList() : null;
        this.outdoorsDates = (outdoorsDates != null) ? outdoorsDates.ToList() : null;
        this.awayDates = (awayDates != null) ? awayDates.ToList() : null;
    }

    // Saves individual LI data to list
    public void addLI(Character character, int dateCount, int points){
        liData.Add(new LIData(character, dateCount, points));
    }

    public string getPlayerName(){
        return playerName;
    }

    public void setPlayerName(string playerName){
        this.playerName = playerName;
    }

    public string getScene(){
        return scene;
    }

    public void setScene(string scene){
        this.scene = scene;
    }

    public int getWeek(){
        return week;
    }

    public void setWeek(int week){
        this.week = week;
    }

    public int getDatesThisWeek(){
        return datesThisWeek;
    }

    public void setDatesThisWeek(int datesThisWeek){
        this.datesThisWeek = datesThisWeek;
    }

    public List<int[]> getLIs(){
        List<int[]> lis = new List<int[]>();

        for(int i = 0; i < liData.Count; i++){
            LIData li = liData[i];
            int[] liArray = {(int)li.getCharacter(), li.getDateCount(), li.getPoints()};

            // if(li != null)
            lis.Add(liArray);
        }

        return lis;
    }

    public List<string> getLocationQueue(Region region){
        switch(region){
            case Region.School: return schoolDates;
            case Region.Elmville: return elmvilleDates;
            case Region.Mordred: return mordredDates;
            case Region.Outdoors: return outdoorsDates;
            case Region.Away: return awayDates;
            default: return null;
        }
    }

    // Pulls specified LI's data
    public int[] GetLIData(int index){
        LIData liData = this.liData[index];
        if(liData == null) return null;

        int[] li = {(int)liData.getCharacter(), liData.getDateCount(), liData.getPoints()};
        return li;
    }
}