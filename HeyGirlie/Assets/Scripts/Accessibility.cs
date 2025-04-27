using UnityEngine;
using UnityEngine.EventSystems;

public class Accessibility : MonoBehaviour
{
    [SerializeField] private GameObject arrowKeyStart;
    [SerializeField] private GameObject arrowKeyPrevious;

    void OnEnable(){
        arrowKeyPrevious = EventSystem.current.currentSelectedGameObject;
        if(arrowKeyStart != null) EventSystem.current.SetSelectedGameObject(arrowKeyStart);
        // Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    void Update(){
        // Debug.Log(EventSystem.current.currentSelectedGameObject.transform.name);
    }

    void OnDisable(){
        if(arrowKeyPrevious != null) EventSystem.current.SetSelectedGameObject(arrowKeyPrevious);
    }

    // void OnDestroy(){
    //     EventSystem.current.SetSelectedGameObject(arrowKeyPrevious);
    // }
}
