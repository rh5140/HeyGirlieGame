using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class SaveProfiles : MonoBehaviour
{
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject[] saves;
    [SerializeField] private Sprite defaultScreenshot;

    private int selectedSave = 0;
    void Start(){
        loadButton.interactable = false;
        deleteButton.interactable = false;
        
        SetScreenshots();
        SetNames();
    }

    public void isSelected(){
        selectedSave = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        bool saveFound = (SaveManager.findSave(selectedSave) != null) ? true : false;

        loadButton.interactable = saveFound;
        deleteButton.interactable = saveFound;
    }

    public void Back(){
        SceneManager.LoadScene("Main Menu");
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
}
