using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}

    private int _week = 1; 
    private int _datesThisWeek = 0;
    [SerializeField] private LoveInterest[] _loveInterests;

    // Separate data structure for ordering by affinity -- for now, just a copy of default
    // Needs more descriptive name..?
    public List<LoveInterest> liQueue;
    public Character priority;
    public Character polyamPartner;
    

    // One queue per region
    // Queue of scene names to load
    public Queue<string> schoolDates;
    public Queue<string> elmvilleDates;
    public Queue<string> bastionCityDates;

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
        bastionCityDates = new Queue<string>();

        DontDestroyOnLoad(this.gameObject);
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
        if(/*add check for Frostkettle condition  met*/)
            liQueue.Add(GetLoveInterest(Character.Frostkettle));
        if (/*add check for 3c condition  met*/)
            liQueue.Add(GetLoveInterest(Character.Trackernara));


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

    public void IncreaseWeek()
    {
        _datesThisWeek = 0;
        _week++;
    }

    public int GetWeek()
    {
        return _week;
    }


}