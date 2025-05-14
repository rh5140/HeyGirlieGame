using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class Menu : MonoBehaviour
{
    protected bool pauseLock = false;
    public UnityEvent OnDestroyEvnt;

    protected EscLock escLock;

    public virtual void Close(){
        Destroy(gameObject);
    }

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
}
