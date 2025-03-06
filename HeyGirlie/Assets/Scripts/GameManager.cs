using UnityEngine;
using UnityEngine.SceneManagement;
using Random=UnityEngine.Random;
using System;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}

    private string _playerName;
    private int _saveProfile;

    private int _week = 1; 
    private int _datesThisWeek = 0;
    [SerializeField] private LoveInterest[] _loveInterests;

    // Separate data structure for ordering by affinity -- for now, just a copy of default
    // Needs more descriptive name..?
    public List<LoveInterest> _liQueue;
    public Character priority;
    public Character polyamPartner;


    // One queue per region
    // Queue of scene names to load
    public Queue<string> schoolDates;
    public Queue<string> elmvilleDates;
    public Queue<string> mordredDates;
    public Queue<string> outdoorsDates;
    public Queue<string> awayDates;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;


        schoolDates = new Queue<string>();
        elmvilleDates = new Queue<string>();
        mordredDates = new Queue<string>();
        outdoorsDates = new Queue<string>();
        awayDates = new Queue<string>();

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update(){ 
        if(Input.GetKeyDown(KeyCode.F1)){
            Save();
        }
    }

    public List<LoveInterest> priorityQueue()
    {
        List<LoveInterest> liQueue = new List<LoveInterest>();
        //loop through all love interests minus polyam routes
        for (int i = 2; i< 8; i++)
        {
            Debug.Log(priority);
            Debug.Log(polyamPartner);
            if ((int)priority != i && i != (int)polyamPartner)
            {
                liQueue.Add(GetLoveInterest((Character)i));
                Debug.Log(priority);
                Debug.Log(liQueue);
            }
        }
        //randomizes non-prio love interests
        int n = liQueue.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            LoveInterest value = liQueue[k];
            liQueue[k] = liQueue[n];
            liQueue[n] = value;
        }
        if (polyamPartner != Character.Kristen) //Kristen is default for non-polyam routes
        {
            liQueue.Add(GetLoveInterest(polyamPartner));
        }
        liQueue.Add(GetLoveInterest(priority));


        //love interest checks are separate and not determined by priority LI; can be triggered by non-prio LI's
        //if(/*add check for Frostkettle condition  met*/)
        //    liQueue.Add(GetLoveInterest(Character.Frostkettle));
        //if (/*add check for 3c condition  met*/)
        //    liQueue.Add(GetLoveInterest(Character.Trackernara));

        liQueue.Reverse();

        return liQueue;
    }

    public LoveInterest GetLoveInterest(Character character)
    {
        switch (character)
        {
            case Character.Fig:
                return _loveInterests[0];
            case Character.Gertie:
                return _loveInterests[1];
            case Character.Kipperlilly:
                return _loveInterests[2];
            case Character.Lucy:
                return _loveInterests[3];
            case Character.Naradriel:
                return _loveInterests[4];
            case Character.Tracker:
                return _loveInterests[5];
            case Character.Frostkettle:
                return _loveInterests[6];
            case Character.Trackernara:
                return _loveInterests[7];
            default:
                return null;
        }
    }

    public void IncreaseDatesThisWeek()
    {
        _datesThisWeek++;
        if (_datesThisWeek > 3) Debug.Log("how did you go on more than 3 dates this week??");
    }

    public int GetDatesThisWeek()
    {
        return _datesThisWeek;
    }

    public void SetDatesThisWeek(int datesThisWeek)
    {
        _datesThisWeek = datesThisWeek;
    }

    public void IncreaseWeek()
    {
        _datesThisWeek = 0;
        _week++;
    }

    public int GetWeek()
    {
        return _week;
    }

    public void SetWeek(int week)
    {
        _week = week;
    }

    public string GetPlayerName()
    {
        return _playerName;
    }

    public void SetPlayerName(string playerName)
    {
        _playerName = playerName;
    }

    public void SetProfile(int profileNum){
        _saveProfile = profileNum;
    }

    public void SetLocationQueues(List<string> schoolDates, List<string> elmvilleDates, List<string> mordredDates, List<string> outdoorsDates, List<string> awayDates){
        this.schoolDates = new Queue<string>(schoolDates);
        this.elmvilleDates = new Queue<string>(elmvilleDates);
        this.mordredDates = new Queue<string>(mordredDates);
        this.outdoorsDates = new Queue<string>(outdoorsDates);
        this.awayDates = new Queue<string>(awayDates);
    }

    public void SetLiQueue(List<int[]> lis){
        _liQueue = new List<LoveInterest>();

        for(int i = 0; i < lis.Count; i++){
            Character character = (Character)lis[i][(int)liInfo.CharacterName];

            _loveInterests[((int)character) - 2].SetDateCount(lis[i][(int)liInfo.DateCount]);
            _loveInterests[((int)character) - 2].SetPoints(lis[i][(int)liInfo.Points]);

            _liQueue.Add(_loveInterests[((int)character) - 2]);
        }
    }

    // Calls to save manager and creates a player data object to add relevant info to save file
    public void Save(){
        PlayerData data = new PlayerData(GetPlayerName(), "Spyre", SceneManager.GetActiveScene().name, GetWeek(), GetDatesThisWeek(), _liQueue,
                                            schoolDates, elmvilleDates, mordredDates, outdoorsDates, awayDates);

        SaveManager.SaveData(data, _saveProfile);
    }
}