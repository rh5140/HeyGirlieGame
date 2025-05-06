using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ArrowNavigation : MonoBehaviour
{  
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject arrowKeyStart;
    [SerializeField] private GameObject arrowKeyPrevious;
    [SerializeField] private bool upDown = false;
    [SerializeField] private bool leftRight = false;

    public void EnableEventSystem(){
        eventSystem.SetActive(true);
        ArrowKeyStart();
    }

    public void DisableEventSystem(){
        eventSystem.SetActive(false);
    }

    public void ArrowKeyStart(){
        try{
            arrowKeyPrevious = EventSystem.current.currentSelectedGameObject;
            if(arrowKeyStart != null) EventSystem.current.SetSelectedGameObject(arrowKeyStart);
        } catch(Exception e) {
            //do nothing
        }
    }

    public void ArrowKeyEnd(){
        if(arrowKeyPrevious != null) EventSystem.current.SetSelectedGameObject(arrowKeyPrevious);
    }

    public void ArrowNav(List<Button> buttons){
        List<Button> valid = new List<Button>();

        for(int i = 0; i < buttons.Count; i++) {
            if(buttons[i].interactable) valid.Add(buttons[i]);
        }

        Navigation nav = arrowKeyStart.GetComponent<Button>().navigation;
        if(arrowKeyStart != null){
            nav.selectOnDown = valid[0];
            nav.selectOnUp = valid[0];
            nav.selectOnLeft = valid[0];
            nav.selectOnRight = valid[0];
            arrowKeyStart.GetComponent<Button>().navigation = nav;
        }

        for(int i = 0; i < valid.Count; i++){
            nav = valid[i].navigation;

            if(upDown){
                nav.selectOnUp = (i == 0) ? valid[valid.Count-1] : valid[i-1];
                nav.selectOnDown = (i == valid.Count-1) ? valid[0] : valid[i+1];
            } else if(leftRight) {
                nav.selectOnLeft = (i == 0) ? valid[valid.Count-1] : valid[i-1];
                nav.selectOnRight = (i == valid.Count-1) ? valid[0] : valid[i+1];
            }

            valid[i].navigation = nav;
        }
    }
}
