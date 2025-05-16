using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DateSelectionInterface : MonoBehaviour
{
    [SerializeField] private List<Button> regions;
    [SerializeField] private Button schoolButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button downtownButton;
    [SerializeField] private Button outdoorsButton;
    [SerializeField] private Button awayButton;
    [SerializeField] private Button background;
    [SerializeField] private AudioTrackManager _atm;
    public void Start()
    {
        List<Button> valid = new List<Button>();
        
        if (GameManager.Instance.DatesThisWeek == 0) SetUpRegions();

        foreach (Region region in Enum.GetValues(typeof(Region)))
        {
            if(GetRegionQueue(region).Count == 0) regions[(int)region].interactable = false;
            else valid.Add(regions[(int)region]);
        }

        gameObject.GetComponent<ArrowNavigation>().ArrowNav(regions);
    }

    public void SetUpRegions()
    {
        GameManager.Instance.awayDates.Clear();
        GameManager.Instance.outdoorsDates.Clear();
        GameManager.Instance.schoolDates.Clear();
        GameManager.Instance.mordredDates.Clear();
        GameManager.Instance.elmvilleDates.Clear();
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
            case Region.Away:
                return GameManager.Instance.awayDates;
            case Region.Outdoors:
                return GameManager.Instance.outdoorsDates;
            case Region.School:
                return GameManager.Instance.schoolDates;
            case Region.Mordred:
                return GameManager.Instance.mordredDates;
            default:
                return GameManager.Instance.elmvilleDates;
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
        _atm.MuteTrack();
        string dateScene = GameManager.Instance.schoolDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectElmvilleRegion()
    {
        _atm.MuteTrack();
        string dateScene = GameManager.Instance.elmvilleDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectMordredRegion()
    {
        _atm.MuteTrack();
        string dateScene = GameManager.Instance.mordredDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectOutdoorsRegion()
    {
        _atm.MuteTrack();
        string dateScene = GameManager.Instance.outdoorsDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
    public void SelectAwayRegion()
    {
        _atm.MuteTrack();
        string dateScene = GameManager.Instance.awayDates.Dequeue();
        SceneManager.LoadScene(dateScene);
    }
}
