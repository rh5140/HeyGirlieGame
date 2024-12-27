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
    public LoveInterest[] liPriority;

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

        // Initializing love interest priority array -- maybe shouldn't be done here?
        liPriority = (LoveInterest[]) _loveInterests.Clone();
        schoolDates = new Queue<string>();
        elmvilleDates = new Queue<string>();
        bastionCityDates = new Queue<string>();

        DontDestroyOnLoad(this.gameObject);
    }

    public LoveInterest SetUpScene(Character character)
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