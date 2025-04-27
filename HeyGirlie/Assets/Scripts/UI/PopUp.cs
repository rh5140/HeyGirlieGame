using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class PopUp : Menu
{
    [SerializeField] private GameObject textInput;
    [SerializeField] private Button okayButton;
    [SerializeField] private TMP_InputField playerName;

    void OnEnable(){
        LockEsc(EscLock.Popup);
        StartCoroutine(SelectInput());
    }

    IEnumerator SelectInput(){
        yield return new WaitForSecondsRealtime(0.25f);
        EventSystem.current.SetSelectedGameObject(textInput);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Popup) gameObject.SetActive(false);
        if(Input.GetKeyUp(KeyCode.Return) && 
        EventSystem.current.currentSelectedGameObject.transform.name == "Input") okayButton.onClick.Invoke();
    }

    // Doesnt work ??
    // void OnSubmit(){
    //     if(EventSystem.current.currentSelectedGameObject.transform.name == "Input") okayButton.onClick.Invoke();
    // }

    void OnDisable(){
        UnlockEsc();
    }

    public void Back(){
        if(playerName != null) playerName.text = "";
        gameObject.SetActive(false);
    }
}
