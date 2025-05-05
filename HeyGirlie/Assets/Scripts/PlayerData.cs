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
    [SerializeField] private string playerName;
    [SerializeField] private string location;
    [SerializeField] private string scene;
    [SerializeField] private int week;
    [SerializeField] private int datesThisWeek;
    [SerializeField] private int priority;
    [SerializeField] private int polyamPartner;
    [SerializeField] private int polyamActive;
    [SerializeField] private int polyamPair;

    [SerializeField] private List<LIData> liData = new List<LIData>();
    [SerializeField] private int ayda;
    
    [SerializeField] private List<string> awayDates;
    [SerializeField] private List<string> outdoorsDates;
    [SerializeField] private List<string> schoolDates;
    [SerializeField] private List<string> mordredDates;
    [SerializeField] private List<string> elmvilleDates;
 
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

    public PlayerData(string playerName) : this(playerName, "Spyre", "Intro", 1, 0, 1, 1, 0, 0, null, 0, null, null, null, null, null) {
        for(int i = 2; i < 8; i++){
            addLI((Character)i, 1, 0);
        }
    }

    public PlayerData(string playerName, string location, string scene, int week, int datesThisWeek, 
                        int priority, int polyamPartner, int polyamActive, int polyamPair, List<LoveInterest> liQueue, int ayda,
                        Queue<string> awayDates, Queue<string> outdoorsDates, Queue<string> schoolDates,
                        Queue<string> mordredDates, Queue<string> elmvilleDates){
        this.playerName = playerName;
        this.location = location;
        this.scene = scene;
        this.week = week;
        this.datesThisWeek = datesThisWeek;
        this.priority = priority;
        this.polyamPartner = polyamPartner;
        this.polyamActive = polyamActive;
        this.polyamPair = polyamPair;
         
        if(liQueue != null){
            foreach(LoveInterest li in liQueue){
                if(li != null) addLI(li.GetCharacter(), li.GetDateCount(), li.GetPoints());
            }
        }
        this.ayda = ayda;

        this.awayDates = (awayDates != null) ? awayDates.ToList() : null;
        this.outdoorsDates = (outdoorsDates != null) ? outdoorsDates.ToList() : null;
        this.schoolDates = (schoolDates != null) ? schoolDates.ToList() : null;
        this.mordredDates = (mordredDates != null) ? mordredDates.ToList() : null;
        this.elmvilleDates = (elmvilleDates != null) ? elmvilleDates.ToList() : null;
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

    public string getLocation(){
        return location;
    }

    public void setLocation(string location){
        this.location = location;
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

    public int getPriority(){
        return priority;
    }

    public void setPriority(int priority){
        this.priority = priority;
    }

    public int getPolyamPartner(){
        return polyamPartner;
    }

    public void setPolyamPartner(int partner){
        this.polyamPartner = partner;
    }

    public int getPolyamActive(){
        return polyamActive;
    }

    public void setPolyamActive(int active){
        this.polyamActive = active;
    }

    public int getPolyamPair(){
        return polyamPair;
    }

    public void setPolyamPair(int pair){
        this.polyamPair = pair;
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

    public bool getAyda(){
        return (ayda == 1 ? true : false);
    }

    public List<string> getLocationQueue(Region region){
        switch(region){
            case Region.Away: return awayDates;
            case Region.Outdoors: return outdoorsDates;
            case Region.School: return schoolDates;
            case Region.Mordred: return mordredDates;
            case Region.Elmville: return elmvilleDates;
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