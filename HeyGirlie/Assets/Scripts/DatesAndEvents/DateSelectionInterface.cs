using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DateSelectionInterface : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("Start!");
        Debug.Log("Dates this week: " + GameManager.Instance.GetDatesThisWeek());
        if (GameManager.Instance.GetDatesThisWeek() == 0) SetUpRegions();
    }

    public void SetUpRegions()
    {
        GameManager.Instance.schoolDates.Clear();
        GameManager.Instance.elmvilleDates.Clear();
        GameManager.Instance.mordredDates.Clear();
        GameManager.Instance.outdoorsDates.Clear();
        GameManager.Instance.awayDates.Clear();
        foreach (LoveInterest li in GameManager.Instance._liQueue)
        {
            // Shouldn't really ever be empty when we're done setting everything up
            if (li.dates != null && li.dates.Length != 0)
            {
                int curDate = li.GetDateCount();
                Date date = li.dates[curDate - 1];
                GetRegionQueue(date.region).Enqueue(date.sceneName);
            }
        }
    }

    private Queue<string> GetRegionQueue(Region region)
    {
        switch (region)
        {
            case Region.School:
                return GameManager.Instance.schoolDates;
            case Region.Elmville:
                return GameManager.Instance.elmvilleDates;
            case Region.Mordred:
                return GameManager.Instance.mordredDates;
            case Region.Outdoors:
                return GameManager.Instance.outdoorsDates;
            default:
                return GameManager.Instance.awayDates;
        }
    }

    // OnClick functions don't support enum parameters :[
    // public void SelectRegion(Region region)
    // {
    //     string dateScene = GetRegionQueue(region).Dequeue();
    //     SceneManager.LoadScene(dateScene);
    // }
    public void SelectSchoolRegion()
    {
        if (GameManager.Instance.schoolDates.Count == 0)
        {
            Debug.Log("No school dates!");
            return;
        }
        string dateScene = GameManager.Instance.schoolDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectElmvilleRegion()
    {
        if (GameManager.Instance.elmvilleDates.Count == 0)
        {
            Debug.Log("No Elmville dates!");
            return;
        }
        string dateScene = GameManager.Instance.elmvilleDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectMordredRegion()
    {
        if (GameManager.Instance.mordredDates.Count == 0)
        {
            Debug.Log("No Mordred dates!");
            return;
        }
        string dateScene = GameManager.Instance.mordredDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectOutdoorsRegion()
    {
        if (GameManager.Instance.outdoorsDates.Count == 0)
        {
            Debug.Log("No Outdoors dates!");
            return;
        }
        string dateScene = GameManager.Instance.outdoorsDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectAwayRegion()
    {
        if (GameManager.Instance.awayDates.Count == 0)
        {
            Debug.Log("No Away dates!");
            return;
        }
        string dateScene = GameManager.Instance.awayDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
}
