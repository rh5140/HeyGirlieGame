using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CharacterProfiles : MonoBehaviour
{
    [SerializeField] private GameObject[] profiles;
    [SerializeField] private GameObject[] progress;

    [SerializeField] private Button backButton;
    [SerializeField] private Button nextButton;

    private int character = (int)Character.Kristen;
    private bool pauseLock = false;
    void Awake()
    {
        if(!GameManager.Instance.pauseLock){
            GameManager.Instance.Pause(true);
            pauseLock = true;
        }

        string scene = SceneManager.GetActiveScene().name;
        string pattern = @"(.+)Date\d+|(Cassandra)";
        Match match = Regex.Match(scene, pattern);

        if(match.Success) {
            character = (match.Groups[1].Value == "") ? GetCharacter(match.Groups[2].Value) : GetCharacter(match.Groups[1].Value);
        } else character = GetCharacter("Kristen");

        LoadProfile();
        SetButtons();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    void OnDestroy(){
        if(pauseLock){
            GameManager.Instance.Pause(false);
            pauseLock = false;
        }
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

    public void Close(){
        Destroy(gameObject);
    }

    private void SetButtons(){
        if(character == (int)Character.Kristen) backButton.interactable = false;
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
                return (int)Character.Kristen;
        }
    }
}
