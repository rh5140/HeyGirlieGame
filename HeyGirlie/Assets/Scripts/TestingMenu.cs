using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
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
        
        // this is repeated in GameManager(170) and there HAS to be a way to make this easier
        PlayerData data = new PlayerData("Kristen", loveInterest+"Date"+week, Int32.Parse(week), 0);
        for(int i = 0; i < 8; i++){
            data.addLI(i, 1, 0);
        }

        SaveManager.SaveData(data, SaveManager.getCount() + 1);

        // Debug.Log(week + " " + loveInterest);

        SceneManager.LoadScene(loveInterest+"Date"+week);
    }
}
