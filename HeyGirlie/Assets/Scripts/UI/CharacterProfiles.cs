using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CharacterProfiles : Menu
{
    [SerializeField] private GameObject[] profiles;
    [SerializeField] private GameObject[] progress;

    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;

    private int character = (int)Character.Cassandra;
    
    void Awake()
    {
        LockEsc(EscLock.Profiles);
        Pause();
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyStart();

        string scene = SceneManager.GetActiveScene().name;
        string pattern = @"(.+)Date\d+|(Cassandra)";
        Match match = Regex.Match(scene, pattern);

        if(match.Success) {
            character = (match.Groups[1].Value == "") ? GetCharacter(match.Groups[2].Value) : GetCharacter(match.Groups[1].Value);
        } else character = GetCharacter("Cassandra");

        LoadProfile();
        SetButtons();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Profiles) Close();
        if(Input.GetKeyDown(KeyCode.LeftArrow) && character != (int)Character.Cassandra) backButton.OnSubmit(null);
        if(Input.GetKeyDown(KeyCode.RightArrow) && character != (int)Character.Naradriel) nextButton.OnSubmit(null);
    }

    void OnDestroy(){
        Unpause();
        UnlockEsc();
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyEnd();
    }

    public void Back(){
        character--;
        LoadProfile();
        SetButtons();
    }

    public void Next(){
        character++;
        LoadProfile();
        SetButtons();
    }

    private void SetButtons(){
        if(character == (int)Character.Cassandra) backButton.interactable = false;
        else backButton.interactable = true;

        if(character == (int)Character.Naradriel) nextButton.interactable = false;
        else nextButton.interactable = true;
    }

    private void LoadProfile(){
        try{
            profiles[character+1].SetActive(false);
        } catch(Exception e) {
            // who cares
        } 
        
        try {
            profiles[character-1].SetActive(false);
        } catch(Exception e){
            // again who cares
        }

        profiles[character].SetActive(true);

        try{
            int dateCount = GameManager.Instance.GetLoveInterest((Character)character).GetDateCount() - 1;
            
            for(int i = 0; i < dateCount; i++){
                progress[i].SetActive(true);
                if(i % 2 != 0) progress[i-1].SetActive(false);
            }
            for(int i = dateCount; i < 8; i++){
                progress[i].SetActive(false);
            }
        } catch(Exception e){
            foreach(GameObject apple in progress){
                apple.SetActive(false);
            }
        }
    }

    private int GetCharacter(string character)
    {
        switch (character)
        {
            case "Cassandra":
                return (int)Character.Cassandra;
            case "Fig":
                return (int)Character.Fig;
            case "Gertie":
                return (int)Character.Gertie;
            case "Kipperlilly":
                return (int)Character.Kipperlilly;
            case "Lucy":
                return (int)Character.Lucy;
            case "Tracker":
                return (int)Character.Tracker;
            case "Naradriel":
                return (int)Character.Naradriel;
            default:
                return (int)Character.Cassandra;
        }
    }
}
