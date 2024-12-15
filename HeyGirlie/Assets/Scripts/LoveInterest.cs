using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LoveInterest : MonoBehaviour
{
    [Range(1,8)][SerializeField] private int _dateCount;
    [SerializeField] private int _points;
    [SerializeField] private int _successThreshold;

    public List<Sprite> expressions = new List<Sprite>();
    public List<AudioClip> voicelines = new List<AudioClip>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    public void IncrementDateCount()
    {
        _dateCount++;
    }

    public void AddPoints(int num)
    {
        _points += num;
    }

    public int GetPoints()
    {
        return _points;
    }

    // Whether promposal succeeds
    public bool promSuccess()
    {
        return _points >= _successThreshold;
    }
}
