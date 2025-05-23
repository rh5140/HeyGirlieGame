using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class SaveGallery : Menu
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private LoadPolaroid[] saves;
    [SerializeField] private GameObject overwritePopup;
    [SerializeField] private GameObject newGamePopup;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject background;

    private Toggle selected;
    private int selectedSave = 0;
    private ArrowNavigation arrowNavigation;

    void Awake(){
        arrowNavigation = gameObject.GetComponent<ArrowNavigation>();
        StartCoroutine(WaitAwake());
    }

    protected IEnumerator WaitAwake(){
        LockEsc(EscLock.Gallery);
        Pause();

        deleteButton.interactable = false;
        loadButton.interactable = false;
        saveButton.interactable = false;
        arrowNavigation.ArrowNav(buttons);

        yield return null;
        CursorManager.Instance.Load(false);
        yield return null;
        arrowNavigation.ArrowKeyStart();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Gallery) Close();
    }

    void OnDestroy(){
        Unpause();
        UnlockEsc();
        arrowNavigation.ArrowKeyEnd();
        if(OnDestroyEvnt != null) OnDestroyEvnt.Invoke();
    }

    public void isSelected(bool toggle){
        if(toggle){
            if(selected != null) selected.isOn = false;
            selectedSave = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
            selected = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();

            Unselect((SaveManager.findSave(selectedSave) != null) ? true : false);
            
            string sceneName = SceneManager.GetActiveScene().name;
            if(!sceneName.Equals("Main Menu") && !sceneName.Equals("DateSelection")) {
                saveButton.interactable = true;
                arrowNavigation.ArrowNav(buttons);
            }
        }
    }

    public void SaveSave(){
        if(SaveManager.findSave(selectedSave) != null){
            overwritePopup.SetActive(true);
        } else {
            newGamePopup.SetActive(true);
        }
    }

    public void OverwriteSave(){
        overwritePopup.SetActive(false);
        newGamePopup.SetActive(true);
    }

    public void NameSave(){
        if(!string.IsNullOrEmpty(playerName.text)) GameManager.Instance.PlayerName = playerName.text;
        GameManager.Instance.SaveProfile = selectedSave;
        StartCoroutine(Save());
    }

    public IEnumerator Save(){
        yield return new WaitUntil(GameManager.Instance.Save);

        saveButton.interactable = false;
        arrowNavigation.ArrowNav(buttons);

        overwritePopup.SetActive(false);
        saves[selectedSave - 1].SetPolaroid();
        // saves[selectedSave - 1].SetScreenshot();
        newGamePopup.SetActive(false);
    }

    public void LoadSave(){
        OnDestroy();

        string scene = SaveManager.LoadData(selectedSave);
        if(scene != null){
            GameObject.Find("AudioTrackManager").GetComponent<AudioTrackManager>().MuteTrack();

            SceneManager.LoadScene(scene);
        } else {
           Debug.Log("how the fuck did you get here");
        }
    }

    public void DeleteSave(){
        if(selectedSave < 1 || selectedSave > 10) return;
        SaveManager.DeleteData(selectedSave);
        saves[selectedSave - 1].SetPolaroid();
        // saves[selectedSave - 1].SetScreenshot();
        Unselect(false);
        selected.isOn = false;
        EventSystem.current.SetSelectedGameObject(background);
    }

    private void Unselect(bool saveFound){
        loadButton.interactable = saveFound;
        deleteButton.interactable = saveFound;
        arrowNavigation.ArrowNav(buttons);
    }
}
