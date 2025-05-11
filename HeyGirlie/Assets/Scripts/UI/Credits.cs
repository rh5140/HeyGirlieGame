using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Credits : Menu
{
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject pageTurn;
    [SerializeField] private GameObject pages;

    void Awake(){
        if(!SceneManager.GetActiveScene().name.Equals("Credits")){
            Pause();
        }
        LockEsc(EscLock.Credits);
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyStart();
    }

    void Start()
    {
        // change credits page every X seconds
        StartCoroutine(RunCredits(2.5f));
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Credits) Close();
    }

    void OnDestroy(){
        Unpause();
        UnlockEsc();
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyEnd();
    }

    public override void Close(){
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
    
    IEnumerator RunCredits(float waitTime)
    {
        int i = 0;
        while (i < (pages.transform.childCount - 1))
        {
            yield return new WaitForSecondsRealtime(waitTime);
            pageTurn.SetActive(true);
            yield return StartCoroutine(PageTurnAnim(0.1f));
            pages.transform.GetChild(i).gameObject.SetActive(false);
            if(pages.transform.GetChild(i + 1) != null)
            {
                pages.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
            
            i++;
        }
    }
    
    IEnumerator PageTurnAnim(float waitTime)
    {
        int i = 0;
        while(i < (pageTurn.transform.childCount))
        {
            yield return new WaitForSecondsRealtime(waitTime);
            pageTurn.transform.GetChild(i).gameObject.SetActive(false);
            if ((i+1) < pageTurn.transform.childCount)
            {
                pageTurn.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
            i++;
        }
    }
}
