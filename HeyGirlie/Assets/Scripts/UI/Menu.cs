using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    protected bool pauseLock = false;
    protected EscLock escLock;

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
