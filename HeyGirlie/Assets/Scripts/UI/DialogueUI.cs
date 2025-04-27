using UnityEngine;
using UnityEngine.UI;
using System;
using Yarn.Unity;

public class DialogueUIButtons : MonoBehaviour
{
    [SerializeField] private GameObject saveProfileMenu;
    [SerializeField] private GameObject characterProfiles;
    [SerializeField] private HGGLineView hggLineView;
    [SerializeField] private GameObject optionsFFButton;

    private bool ffActive = false;

    public void Awake(){
        optionsFFButton.GetComponent<Button>().interactable = false;
        optionsFFButton.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (SettingManager.Instance.fastForwardActive) ? 80f : 0f);
    }

    public void Save(){
        GameManager.Instance.Save();
    }

    public void Load(){
        Instantiate(saveProfileMenu);
    }

    public void FastForward(){
        ffActive = !ffActive;
        SettingManager.Instance.fastForwardActive = ffActive;

        hggLineView.SetAutoAdvanced(ffActive);
        
        if(ffActive) hggLineView.SetSpeed((float) (hggLineView.GetSpeed()*2));
        else hggLineView.SetSpeed((float) (hggLineView.GetSpeed()*0.5));

        hggLineView.UserRequestedViewAdvancement();
    }

    public void CharacterProfiles(){
        Instantiate(characterProfiles);
    }
}
