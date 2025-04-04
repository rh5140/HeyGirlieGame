using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DateSelectionInterface : MonoBehaviour
{
    [SerializeField] private Button schoolButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button downtownButton;
    [SerializeField] private Button outdoorsButton;
    [SerializeField] private Button awayButton;
    public void Start()
    {
        // Debug.Log("Start!");
        // Debug.Log("Dates this week: " + GameManager.Instance.GetDatesThisWeek());
        if (GameManager.Instance.GetDatesThisWeek() == 0) SetUpRegions();

        if (GameManager.Instance.schoolDates.Count == 0) schoolButton.interactable = false;
        if (GameManager.Instance.mordredDates.Count == 0) homeButton.interactable = false;
        if (GameManager.Instance.elmvilleDates.Count == 0) downtownButton.interactable = false;
        if (GameManager.Instance.outdoorsDates.Count == 0) outdoorsButton.interactable = false;
        if (GameManager.Instance.awayDates.Count == 0) awayButton.interactable = false;
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
        string dateScene = GameManager.Instance.schoolDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectElmvilleRegion()
    {
        string dateScene = GameManager.Instance.elmvilleDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectMordredRegion()
    {
        string dateScene = GameManager.Instance.mordredDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectOutdoorsRegion()
    {
        string dateScene = GameManager.Instance.outdoorsDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectAwayRegion()
    {
        string dateScene = GameManager.Instance.awayDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
}
