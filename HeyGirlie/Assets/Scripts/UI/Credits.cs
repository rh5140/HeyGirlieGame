using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : Menu
{
    [SerializeField] private GameObject creditsMenu;
    void Awake(){
        if(!SceneManager.GetActiveScene().name.Equals("Credits")){
            Pause();
        }
        LockEsc(EscLock.Credits);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Credits) Close();
    }

    void OnDestroy(){
        Unpause();
        UnlockEsc();
    }

    public void Close(){
        if(!SceneManager.GetActiveScene().name.Equals("Credits")) Destroy(creditsMenu);
        else {
            #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_WEBGL)
                SceneManager.LoadScene("Main Menu");
            #else
                Application.Quit();
            #endif
        }
    }
}
