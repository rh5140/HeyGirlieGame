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

        // Debug.Log(week + " " + loveInterest);

        SceneManager.LoadScene(loveInterest+"Date"+week);
    }
}
