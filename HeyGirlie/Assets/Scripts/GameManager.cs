using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {get {return _instance;}}

    private int week = 1;
    private int rank = 1;

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

    public void IncreaseWeek()
    {
        week++;
    }

    public int GetWeek()
    {
        return week;
    }

    public void IncreaseRank(string name)
    {
        rank++;
    }

    public int GetRank(string name)
    {
        return rank;
    }
}