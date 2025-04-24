using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class SaveProfiles : MonoBehaviour//, IDeselectHandler
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private LoadPolaroid[] saves;
    [SerializeField] private GameObject overwritePopup;
    [SerializeField] private GameObject newGamePopup;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject background;

    private GameObject selected;
    private int selectedSave = 0;

    private bool pauseLock = false;
    void Awake(){
        if(!GameManager.Instance.pauseLock){
            GameManager.Instance.Pause(true);
            pauseLock = true;
        }

        loadButton.interactable = false;
        deleteButton.interactable = false;
        saveButton.interactable = false;
    }

    void Update(){
        if(EventSystem.current.currentSelectedGameObject == null){
            loadButton.interactable = false;
            deleteButton.interactable = false;
            saveButton.interactable = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    void OnDestroy(){
        if(pauseLock){
            GameManager.Instance.Pause(false);
            pauseLock = false;
        }
    }

    public void isSelected(){
        selectedSave = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);

        Unselect((SaveManager.findSave(selectedSave) != null) ? true : false);
        if(!SceneManager.GetActiveScene().name.Equals("Main Menu")) saveButton.interactable = true;
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
        EventSystem.current.SetSelectedGameObject(background);
    }

    private void Unselect(bool saveFound){
        loadButton.interactable = saveFound;
        deleteButton.interactable = saveFound;
    }
}
