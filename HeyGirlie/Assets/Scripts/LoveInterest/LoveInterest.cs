using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LoveInterest : MonoBehaviour
{
    [SerializeField] private Character _character;
    [Range(1,9)][SerializeField] private int _dateCount;
    [SerializeField] protected int _points;
    [SerializeField] protected int _successThreshold;
    [SerializeField] private int _datesForSuccess;
    [SerializeField] public Date[] dates;
    [SerializeField] private string[] _locationHints;

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

    public string GetDateHint()
    {
        return _locationHints[_dateCount - 1];
    }

    // Whether Spring Fling date is successful at the end
    public virtual bool SucceedEnding()
    {
        return (_points >= _successThreshold) && (_dateCount >= _datesForSuccess);
    }

    public string GetName()
    {
        return _character.ToString();
    }
}
