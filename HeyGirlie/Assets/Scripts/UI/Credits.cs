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
        // change credits page every 3 seconds
        StartCoroutine(RunCredits(3.0f));
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

    /*
    // the below function is WIP, it needs a coroutine so frames change every X seconds
    private void PageTurnAnim()
    {
        //enable the page turn object
        pageTurn.SetActive(true);
        //disable the current frame and enable the next one
        for (int i = 0; i < 6; i++)
        {
            pageTurn.transform.GetChild(i).gameObject.SetActive(false);
            if(pageTurn.transform.GetChild(i + 1) != null)
            {
                pageTurn.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
        }
    }
    */
    
    IEnumerator RunCredits(float waitTime)
    {
        int i = 0;
        while (i < 6)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            pages.transform.GetChild(i).gameObject.SetActive(false);
            if(pages.transform.GetChild(i + 1) != null)
            {
                pages.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
            i++;
        }
    }
    

}
