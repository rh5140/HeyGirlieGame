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
        
        name.text = (data != null) ? data.PlayerName : "";
        location.text = (data != null) ? data.Location : "";
    }
}
