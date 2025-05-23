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

    private string _playerName = "Kristen";
    public string PlayerName {
        get {return _playerName;}
        set {_playerName = value;}
    }

    private string _location = "Spyre";
    public string Location {
        get {return _location;}
        set {_location = value;}
    }
    
    private int _saveProfile = 0;
    public int SaveProfile {
        get {return _saveProfile;}
        set {_saveProfile = value;}
    }
    
    [SerializeField] private GameObject savePopup;

    // public string _lastMenu = "Main Menu";

    private int _week = 1;
    public int Week {
        get {return _week;}
        set {_week = value;}
    }

    private int _datesThisWeek = 0;
    public int DatesThisWeek {
        get {return _datesThisWeek;}
        set {_datesThisWeek = value;}
    }

    [SerializeField] private LoveInterest[] _loveInterests;

    // Separate data structure for ordering by affinity -- for now, just a copy of default
    // Needs more descriptive name..?
    public List<LoveInterest> _liQueue;
    public Character priority;
    public Character Priority {
        get {return priority;}
        set {priority = value;}
    }

    public Character polyamPartner;
    public Character PolyamPartner {
        get {return polyamPartner;}
        set {polyamPartner = value;}
    }

    // Checking for polyam condition
    [SerializeField] private bool _polyamActive;
    public bool PolyamActive {
        get {return _polyamActive;}
        set {_polyamActive = value;}
    }

    [SerializeField] private Character _polyamPair;
    public Character PolyamPair {
        get {return _polyamPair;}
        set {_polyamPair = value;}
    }


    // One queue per region
    // Queue of scene names to load
    public Queue<string> awayDates;
    public Queue<string> outdoorsDates;
    public Queue<string> schoolDates;
    public Queue<string> mordredDates;
    public Queue<string> elmvilleDates;

    public bool pauseLock = false;
    public EscLock escLock = EscLock.Dropdown;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        _saveProfile = (_saveProfile == 0) ? SaveManager.getCount() : _saveProfile;

        schoolDates = new Queue<string>();
        elmvilleDates = new Queue<string>();
        mordredDates = new Queue<string>();
        outdoorsDates = new Queue<string>();
        awayDates = new Queue<string>();

        DontDestroyOnLoad(this.gameObject);
    }

    void Update(){
        // Debug.Log(Time.timeScale);
    }

    public void Pause(bool pause){
        if(pause) {
            Time.timeScale = 0;
            pauseLock = true;
        } else {
            Time.timeScale = 1;
            pauseLock = false;
        }
    }

    public List<LoveInterest> priorityQueue()
    {
        List<LoveInterest> liQueue = new List<LoveInterest>();
        //loop through all love interests minus polyam routes
        for (int i = 2; i< 8; i++)
        {
            if ((int)priority != i && i != (int)polyamPartner)
            {
                liQueue.Add(GetLoveInterest((Character)i));
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
        //Kristen is default for non-polyam routes
        if (polyamPartner != Character.Kristen) liQueue.Add(GetLoveInterest(polyamPartner));
        if(priority != Character.Kristen) liQueue.Add(GetLoveInterest(priority));
        
        //love interest checks are separate and not determined by priority LI; can be triggered by non-prio LI's
        if(_polyamActive){
            liQueue.Add(GetLoveInterest(_polyamPair));
        }

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
            case Character.Tracker:
                return _loveInterests[4];
            case Character.Naradriel:
                return _loveInterests[5];
            case Character.Frostkettle:
                return _loveInterests[6];
            case Character.Trackernara:
                return _loveInterests[7];
            case Character.Ayda:
                return _loveInterests[8];
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

    public void SetLocationQueues(List<string> awayDates, List<string> outdoorsDates, List<string> schoolDates, List<string> mordredDates, List<string> elmvilleDates){
        this.awayDates = new Queue<string>(awayDates);
        this.outdoorsDates = new Queue<string>(outdoorsDates);
        this.schoolDates = new Queue<string>(schoolDates);
        this.mordredDates = new Queue<string>(mordredDates);
        this.elmvilleDates = new Queue<string>(elmvilleDates);
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

    public void SetAyda(bool success){
        ((AydaLI) GetLoveInterest(Character.Ayda)).SetAydaDate7(success);
    }

    public void SetFigW4(bool success)
    {
        ((FigLI) GetLoveInterest(Character.Fig)).SetFigW4(success);
    }

    // Calls to save manager and creates a player data object to add relevant info to save file
    public bool Save(){
        PlayerData data = new PlayerData(_playerName, _location, SceneManager.GetActiveScene().name, _week, _datesThisWeek, 
                                            (int)priority, (int)polyamPartner, (_polyamActive ? 1 : 0), (int)_polyamPair, _liQueue, (((AydaLI) GetLoveInterest(Character.Ayda)).GetAydaDate7() ? 1 : 0), (((FigLI)GetLoveInterest(Character.Fig)).GetFigW4() ? 1 : 0),
                                            awayDates, outdoorsDates, schoolDates, mordredDates, elmvilleDates);

        SaveManager.SaveData(data, _saveProfile);
        Instantiate(savePopup);
        return true;
    }

    public void SetPolyamActive(Character polyam)
    {
        _polyamActive = true;
        _polyamPair = polyam;
        _liQueue.Insert(0, GetLoveInterest(polyam)); // Add to start of liQueue
    }

    public LoveInterest[] GetLIArray()
    {
        return _loveInterests;
    }
}