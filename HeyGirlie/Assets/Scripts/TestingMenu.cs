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
        foreach(LoveInterest li in liQueue) if(li != null) Debug.Log(li.GetCharacter());

        PlayerData data = new PlayerData("Kristen", loveInterest+"Date"+week, Int32.Parse(week), 0, GameManager.Instance.priorityQueue());

        int newProfileNum = SaveManager.getCount() + 1;
        // Debug.Log("newProfileNum = " + newProfileNum);
        if(newProfileNum <= 10)
            SaveManager.SaveData(data, newProfileNum);

        // if(SaveManager.NewData(null) != null)
            SceneManager.LoadScene(loveInterest+"Date"+week);
        // else MainMenu.openMaxProfilePopup();
    }
}
