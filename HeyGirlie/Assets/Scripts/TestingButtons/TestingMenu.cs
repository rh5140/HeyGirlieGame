using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class TestingMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private TMP_Dropdown weekDropdown;
    [SerializeField] private TMP_Dropdown liDropdown;
    [SerializeField] private TMP_Dropdown eventDropdown;
  
      public void Open(){
        menu.SetActive(true);
        menuButton.SetActive(false);
    }

    public void Close(){
        menu.SetActive(false);
        menuButton.SetActive(true);
    }

    public void Enter(){
        string week = weekDropdown.options[weekDropdown.value].text;
        string loveInterest = liDropdown.options[liDropdown.value].text;
        
        List<LoveInterest> liQueue = GameManager.Instance.priorityQueue();
        
        GameObject.Find("AudioTrackManager").GetComponent<AudioTrackManager>().MuteTrack();
        // if(SaveManager.NewData(null) != null)
        if(loveInterest.Equals("FigAyda (alt date 8)"))
        {
            SceneManager.LoadScene("FigDate8 Alt");
        } else
        {
            SceneManager.LoadScene(loveInterest + "Date" + week);
        }
        // else MainMenu.openMaxProfilePopup();
    }

    public void Event()
    {
        string eventName = eventDropdown.options[eventDropdown.value].text;
        string sceneName = "";
        int week = 0;
        switch (eventName)
        {
            case "Museum (after Week 2)":
                sceneName = "Week2Event";
                week = 3;
                break;
            case "Music Festival (after Week 4)":
                sceneName = "Week4Event";
                week = 5;
                break;
            case "Activities Fair (after Week 6)":
                sceneName = "Week6Event";
                week = 7;
                break;
            default:
                sceneName = "SpringFling";
                week = 9;
                break;
        }
        GameObject.Find("AudioTrackManager").GetComponent<AudioTrackManager>().MuteTrack();
        SceneManager.LoadScene(sceneName);
        // List<LoveInterest> liQueue = GameManager.Instance.priorityQueue();
        // foreach(LoveInterest li in liQueue) if(li != null) Debug.Log(li.GetCharacter());
    }
}
