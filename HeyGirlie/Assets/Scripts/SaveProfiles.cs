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

    private int selectedSave;
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
        PlayerData data = SaveManager.LoadData(selectedSave);
        if(data != null){
            SceneManager.LoadScene(data.getScene());
        } else {
           Debug.Log("how the fuck did you get here");
        }
    }

    public void DeleteSave(){
        SaveManager.DeleteData(selectedSave);
        SetScreenshots();
        SetNames();
    }

    private void SetScreenshots(){
        for(int save = 1; save <= saves.Length; save++){
            Sprite screenshot = SaveManager.getScreenshot(save);

            saves[save - 1].GetComponent<Image>().sprite = (screenshot != null) ? screenshot : defaultScreenshot;
        }
    }

    private void SetNames(){
        for(int save = 1; save <= saves.Length; save++){
            string filePath = SaveManager.findSave(save);

            saves[save - 1].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = filePath;
        }
    }
}
