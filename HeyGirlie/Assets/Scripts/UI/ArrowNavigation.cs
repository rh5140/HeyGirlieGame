using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowNavigation : MonoBehaviour
{  
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private GameObject arrowKeyStart;
    [SerializeField] private GameObject arrowKeyPrevious;

    public void EnableEventSystem(){
        eventSystem.SetActive(true);
        ArrowKeyStart();
    }

    public void DisableEventSystem(){
        eventSystem.SetActive(false);
    }

    protected void ArrowKeyStart(){
        arrowKeyPrevious = EventSystem.current.currentSelectedGameObject;
        if(arrowKeyStart != null) EventSystem.current.SetSelectedGameObject(arrowKeyStart);
    }

    protected void ArrowKeyEnd(){
        if(arrowKeyPrevious != null) EventSystem.current.SetSelectedGameObject(arrowKeyPrevious);
    }
}
