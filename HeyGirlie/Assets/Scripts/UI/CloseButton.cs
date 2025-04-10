using UnityEngine;

public class CloseButton : MonoBehaviour
{
    private static CloseButton _instance;
    public static CloseButton Instance {get {return _instance;}}
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void QuitGame(){
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_WEBGL)
            SceneManager.LoadScene("Main Menu");
        #else
            Application.Quit();
        #endif
    }
}
