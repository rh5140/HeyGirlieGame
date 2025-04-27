using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Menu : MonoBehaviour
{
    protected bool pauseLock = false;
    protected EscLock escLock;

    [SerializeField] private GameObject arrowKeyStart;
    [SerializeField] private GameObject arrowKeyPrevious;

    protected void Pause(){
        if(!GameManager.Instance.pauseLock){
            GameManager.Instance.Pause(true);
            pauseLock = true;
        }
    }

    protected void Unpause(){
        if(pauseLock){
            GameManager.Instance.Pause(false);
            pauseLock = false;
        }
    }

    protected void LockEsc(EscLock escLock) {
        this.escLock = GameManager.Instance.escLock;
        GameManager.Instance.escLock = escLock;
    }

    protected void UnlockEsc(){
        GameManager.Instance.escLock = escLock;
    }

    protected void ArrowKeyStart(){
        arrowKeyPrevious = EventSystem.current.currentSelectedGameObject;
        if(arrowKeyStart != null) EventSystem.current.SetSelectedGameObject(arrowKeyStart);
    }

    protected void ArrowKeyEnd(){
        if(arrowKeyPrevious != null) EventSystem.current.SetSelectedGameObject(arrowKeyPrevious);
    }
}
