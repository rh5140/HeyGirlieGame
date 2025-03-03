using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LoveInterest : MonoBehaviour
{
    [SerializeField] private Character _character;
    [Range(1,9)][SerializeField] private int _dateCount;
    [SerializeField] private int _points;
    [SerializeField] private int _successThreshold;

    [SerializeField] public Date[] dates;

    // Note: I don't really want to store these in the Love Interest -- make separate class for the sprites?
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

    public Character GetCharacter()
    {
        return _character;
    }

    public void SetCharacter(Character character)
    {
        _character = character;
    }

    public int GetDateCount()
    {
        return _dateCount;
    }

    public void SetDateCount(int dateCount)
    {
        _dateCount = dateCount;
    }

    public void AddPoints(int num)
    {
        _points += num;
    }

    public int GetPoints()
    {
        return _points;
    }

    public void SetPoints(int points)
    {
        _points = points;
    }

    // Whether Spring Fling date is successful at the end
    public bool SucceedEnding()
    {
        return _points >= _successThreshold;
    }
}
