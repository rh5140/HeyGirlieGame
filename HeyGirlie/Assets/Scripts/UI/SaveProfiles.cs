using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class SaveProfiles : Menu
{
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

    void Awake(){
        LockEsc(EscLock.Gallery);
        Pause();
        ArrowKeyStart();

        loadButton.interactable = false;
        deleteButton.interactable = false;
        saveButton.interactable = false;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Gallery) Close();
    }

    void OnDestroy(){
        Unpause();
        UnlockEsc();
        ArrowKeyEnd();
    }

    public void isSelected(bool toggle){
        if(toggle){
            if(selected != null) selected.isOn = false;
            selectedSave = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
            selected = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();

            Unselect((SaveManager.findSave(selectedSave) != null) ? true : false);
            if(!SceneManager.GetActiveScene().name.Equals("Main Menu")) saveButton.interactable = true;
        }
    }

    public void Close(){
        Destroy(gameObject);
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
        if(!string.IsNullOrEmpty(playerName.text)) GameManager.Instance.SetPlayerName(playerName.text);
        GameManager.Instance.SetProfile(selectedSave);
        StartCoroutine(Save());
    }

    public IEnumerator Save(){
        yield return new WaitUntil(GameManager.Instance.Save);
        saveButton.interactable = false;
        overwritePopup.SetActive(false);
        saves[selectedSave - 1].SetName();
        saves[selectedSave - 1].SetScreenshot();
        newGamePopup.SetActive(false);
    }

    public void LoadSave(){
        string scene = SaveManager.LoadData(selectedSave);
        if(scene != null){
            SceneManager.LoadScene(scene);
        } else {
           Debug.Log("how the fuck did you get here");
        }
    }

    public void DeleteSave(){
        if(selectedSave < 1 || selectedSave > 10) return;
        SaveManager.DeleteData(selectedSave);
        saves[selectedSave - 1].SetName();
        saves[selectedSave - 1].SetScreenshot();
        Unselect(false);
        selected.isOn = false;
        EventSystem.current.SetSelectedGameObject(background);
    }

    private void Unselect(bool saveFound){
        loadButton.interactable = saveFound;
        deleteButton.interactable = saveFound;
    }
}
