using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class LoadPolaroid : MonoBehaviour
{
    [SerializeField] private int profileNum;
    [SerializeField] private Sprite defaultScreenshot;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text location;

    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    void Awake()
    {
        SetName();
        SetScreenshot();
    }

    public void SetScreenshot(){
        Sprite screenshot = SaveManager.getScreenshot(profileNum);

        image.sprite = (screenshot != null) ? screenshot : defaultScreenshot;
    }

    public void SetName(){
        PlayerData data = SaveManager.findSave(profileNum);
        
        name.text = (data != null) ? data.getPlayerName() : "";
        location.text = (data != null) ? data.getLocation() : "";
    }

    public void Selected(bool toggle){
        if(toggle){
            loadButton.interactable = true;
            deleteButton.interactable = true;
            saveButton.interactable = true;
        } else {
            loadButton.interactable = false;
            deleteButton.interactable = false;
            saveButton.interactable = false;
        }
    }
}
