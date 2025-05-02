using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowNavigation : MonoBehaviour
{  
    [SerializeField] private GameObject arrowKeyStart;
    [SerializeField] private GameObject arrowKeyPrevious;

    protected void ArrowKeyStart(){
        arrowKeyPrevious = EventSystem.current.currentSelectedGameObject;
        if(arrowKeyStart != null) EventSystem.current.SetSelectedGameObject(arrowKeyStart);
    }

    protected void ArrowKeyEnd(){
        if(arrowKeyPrevious != null) EventSystem.current.SetSelectedGameObject(arrowKeyPrevious);
    }
}
