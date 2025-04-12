using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private bool pauseLock = false;
    [SerializeField] private GameObject creditsMenu;
    void Awake(){
        if(!SceneManager.GetActiveScene().name.Equals("Credits") && !GameManager.Instance.pauseLock){
            GameManager.Instance.Pause(true);
            pauseLock = true;
        }
    }

    void OnDestroy(){
        if(pauseLock){
            GameManager.Instance.Pause(false);
            pauseLock = false;
        }
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
