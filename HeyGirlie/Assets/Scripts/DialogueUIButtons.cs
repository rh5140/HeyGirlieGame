using UnityEngine;

public class DialogueUIButtons : MonoBehaviour
{
    [SerializeField] private GameObject saveProfileMenu;
    [SerializeField] private GameObject settingsMenu;

    public void Save(){
        GameManager.Instance.Save();
    }

    public void Load(){
        Instantiate(saveProfileMenu);
    }

    public void FastForward(){
        Debug.Log("help what is this supposed to do");
    }

    public void Settings(){
        Instantiate(settingsMenu);
    }
}
