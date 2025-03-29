using UnityEngine;
using System;
using Yarn.Unity;

public class DialogueUIButtons : MonoBehaviour
{
    [SerializeField] private GameObject saveProfileMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private HGGLineView hggLineView;

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

    public void Settings(){
        Instantiate(settingsMenu);
    }

    // private void Unselect(){
    //     EventSystem.current.SetSelectedGameObject(null);
    // }
}
