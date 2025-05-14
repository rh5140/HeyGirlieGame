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
    public string PlayerName {
        get {return playerName;}
        set {playerName = value;}
    }

    [SerializeField] private string location;
    public string Location {
        get {return location;}
        set {location = value;}
    }
    
    [SerializeField] private string scene;
    public string Scene {
        get {return scene;}
        set {scene = value;}
    }
    
    [SerializeField] private int week;
    public int Week {
        get {return week;}
        set {week = value;}
    }
    
    [SerializeField] private int datesThisWeek;
    public int DatesThisWeek {
        get {return datesThisWeek;}
        set {datesThisWeek = value;}
    }
    
    [SerializeField] private int priority;
    public int Priority {
        get {return priority;}
        set {priority = value;}
    }
    
    [SerializeField] private int polyamPartner;
    public int PolyamPartner {
        get {return polyamPartner;}
        set {polyamPartner = value;}
    }
    
    [SerializeField] private int polyamActive;
    public int PolyamActive {
        get {return polyamActive;}
        set {polyamActive = value;}
    }
    
    [SerializeField] private int polyamPair;
    public int PolyamPair {
        get {return polyamPair;}
        set {polyamPair = value;}
    }

    [SerializeField] private List<LIData> liData = new List<LIData>();
    [SerializeField] private int ayda;
    public bool Ayda {
        get {return (ayda == 1 ? true : false);}
    }

    [SerializeField] private int figW4;
    public bool FigW4 {
        get {return (figW4 == 1 ? true : false);}
    }

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

    public PlayerData(string playerName) : this(playerName, "Spyre", "Intro", 1, 0, 1, 1, 0, 0, null, 0, 0, null, null, null, null, null) {
        for(int i = 2; i < 8; i++){
            addLI((Character)i, 1, 0);
        }
    }

    public PlayerData(string playerName, string location, string scene, int week, int datesThisWeek, 
                        int priority, int polyamPartner, int polyamActive, int polyamPair, List<LoveInterest> liQueue, int ayda, int figW4,
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
        this.figW4 = figW4;

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