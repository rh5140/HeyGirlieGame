using UnityEngine;
using UnityEngine.UI;
using System;
using Yarn.Unity;

public class DialogueUIButtons : MonoBehaviour
{
    [SerializeField] private GameObject saveProfileMenu;
    [SerializeField] private GameObject characterProfiles;
    [SerializeField] private HGGLineView hggLineView;
    [SerializeField] private Toggle fastFowardButton;

    public void Awake(){
        fastFowardButton.interactable = false;
    }

    public void Save(){
        GameManager.Instance.Save();
    }

    public void Load(){
        Instantiate(saveProfileMenu);
    }

    public void FastForward(bool value){
        hggLineView.SetAutoAdvanced(value);
        
        if(value) hggLineView.SetSpeed((float) (hggLineView.GetSpeed()*2));
        else hggLineView.SetSpeed((float) (hggLineView.GetSpeed()*0.5));

        hggLineView.UserRequestedViewAdvancement();
    }

    public void CharacterProfiles(){
        Instantiate(characterProfiles);
    }

    // private void Unselect(){
    //     EventSystem.current.SetSelectedGameObject(null);
    // }
}
