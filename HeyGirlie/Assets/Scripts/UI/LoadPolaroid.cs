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
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text location;
    void Awake()
    {
        SetPolaroid();
    }

    public void SetPolaroid(){
        PlayerData data = SaveManager.findSave(profileNum);
        
        name.text = (data != null) ? data.PlayerName : "";
        location.text = (data != null) ? data.Location : "";

        #if(UNITY_WEBGL)
            if(data == null) image.sprite = defaultScreenshot;
            else {
                int rand = UnityEngine.Random.Range(0, 1);
                image.sprite = backgrounds[rand];
            }
        #else
            Sprite screenshot = SaveManager.getScreenshot(profileNum);
            image.sprite = (screenshot != null) ? screenshot : defaultScreenshot;
        #endif
    }
}
