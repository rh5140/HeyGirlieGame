using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class SaveProfiles : MonoBehaviour//, IDeselectHandler
{
    [SerializeField] private GameObject saveProfilesMenu;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject[] saves;
    [SerializeField] private Sprite defaultScreenshot;
    [SerializeField] private GameObject overwritePopup;
    [SerializeField] private GameObject newGamePopup;
    [SerializeField] private TMP_InputField playerName;

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

        SetScreenshots();
        SetNames();
    }

    void Update(){
        if(EventSystem.current.currentSelectedGameObject == null){
            loadButton.interactable = false;
            deleteButton.interactable = false;
            saveButton.interactable = false;
        }
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
        Destroy(saveProfilesMenu);
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
        SetNames();
        SetScreenshots();
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
        SetScreenshots();
        SetNames();
        Unselect(false);
    }

    private void SetScreenshots(){
        for(int save = 1; save <= saves.Length; save++){
            Sprite screenshot = SaveManager.getScreenshot(save);

            saves[save - 1].transform.Find("Image").GetComponent<Image>().sprite = (screenshot != null) ? screenshot : defaultScreenshot;
        }
    }

    private void SetNames(){
        for(int save = 1; save <= saves.Length; save++){
            PlayerData data = SaveManager.findSave(save);

            saves[save - 1].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = (data != null) ? data.getPlayerName() : "";
            saves[save - 1].transform.Find("Location").GetComponent<TextMeshProUGUI>().text = (data != null) ? data.getLocation() : "";
        }
    }

    private void Unselect(bool saveFound){
        // EventSystem.current.SetSelectedGameObject(null);

        loadButton.interactable = saveFound;
        deleteButton.interactable = saveFound;
    }
}
