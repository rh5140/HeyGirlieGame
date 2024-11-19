using UnityEngine;

public enum Character
{
    Kristen,
    Cassandra,
    Fig,
    Gertie,
    Kipperlilly,
    Lucy,
    Naradriel,
    Tracker,
    Frostkettle,
    Trackernara,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}

    private int week = 1; // Temporary
    [SerializeField] private LoveInterest[] loveInterests;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public LoveInterest SetUpScene(Character character)
    {
        switch (character)
        {
            case Character.Fig:
                return loveInterests[0];
            case Character.Gertie:
                return loveInterests[1];
            case Character.Kipperlilly:
                return loveInterests[2];
            case Character.Lucy:
                return loveInterests[3];
            case Character.Naradriel:
                return loveInterests[4];
            case Character.Tracker:
                return loveInterests[5];
            case Character.Frostkettle:
                return loveInterests[6];
            case Character.Trackernara:
                return loveInterests[7];
            default:
                return null;
        }
    }

    public void IncreaseWeek()
    {
        week++;
    }

    public int GetWeek()
    {
        return week;
    }

}