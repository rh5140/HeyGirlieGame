using UnityEngine;

public class Polyam : LoveInterest
{
    public LoveInterest _li1;
    public LoveInterest _li2;
    [SerializeField] private int _li1PointsThreshold;
    [SerializeField] private int _li2PointsThreshold;

    // Be careful about _dateCount

    public bool MeetPolyamConditions()
    {
        // Dates are 1-indexed, and GetDateCount is the NEXT date number
        return     _li1.GetDateCount() > 4 
                && _li2.GetDateCount() > 4
                && _li1.GetPoints() >= _li1PointsThreshold
                && _li2.GetPoints() >= _li2PointsThreshold;
    }
}
