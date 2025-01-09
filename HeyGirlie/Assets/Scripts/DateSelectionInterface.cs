using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DateSelectionInterface : MonoBehaviour
{
    public void Start()
    {
        Debug.Log("Start!");
        if (GameManager.Instance.GetDatesThisWeek() == 0) SetUpRegions();
    }

    public void SetUpRegions()
    {
        foreach (LoveInterest li in GameManager.Instance.liQueue)
        {
            // Shouldn't really ever be empty when we're done setting everything up
            if (li.dates != null && li.dates.Length != 0)
            {
                // Bounds checking
                int curDate = li.GetDateCount();
                if (li.dates.Length >= curDate)
                {
                    Date date = li.dates[curDate - 1];
                    GetRegionQueue(date.region).Enqueue(date.sceneName);
                    Debug.Log("Add " + date.sceneName + " to " + date.region);
                }
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



    // TEMPORARY FOR TESTING
    // Later -- parameter is Character enum defined in GameManager.cs -> load relevant scene
    public void ChangeScene()
    {
        SceneManager.LoadScene("Test Route");
    }

    // TEMPORARY FOR TESTING
    public void CassandraScene()
    {
        SceneManager.LoadScene("Cassandra");
    }
}
