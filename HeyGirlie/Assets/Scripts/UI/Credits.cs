using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Credits : Menu
{
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private Transform leftPageBefore;
    [SerializeField] private Transform leftPageAfter;
    [SerializeField] private Transform rightPageBottomBefore;
    [SerializeField] private GameObject rightPageBottomAfter;
    [SerializeField] private Image rightPageTop;
    [SerializeField] private Sprite[] pageBottomFrames;
    [SerializeField] private Sprite[] pageTopFrames;
    [SerializeField] private Sprite[] closeBookFrames;
    [SerializeField] private Sprite[] closePageFrames;
    [SerializeField] private Sprite[] closeCoverFrames;
    [SerializeField] private Sprite rightPage;

    void Awake(){
        Pause();
        LockEsc(EscLock.Credits);
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyStart();
    }

    void Start()
    {
        // change credits page every X seconds
        // StartCoroutine(RunCredits(5f));
        StartCoroutine(EndCredits());
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.escLock == EscLock.Credits) Close();
    }

    void OnDestroy(){
        Unpause();
        UnlockEsc();
        gameObject.GetComponent<ArrowNavigation>().ArrowKeyEnd();
        if(OnDestroyEvnt != null) OnDestroyEvnt.Invoke();
    }

    public override void Close(){
        Destroy(creditsMenu);
    }

    IEnumerator EndCredits(){
        yield return StartCoroutine(RunCredits(2.5f));

        Image leftPageAfterImage = leftPageAfter.gameObject.GetComponent<Image>();
        Image rightPageBottomAfterImage = rightPageBottomAfter.GetComponent<Image>();
        // Image rightPageBottomBeforeImage = rightPageBottomBefore.GetComponent<Image>();
        
        int index = 0;
        for (int i = 0; i < closeBookFrames.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            leftPageAfterImage.sprite = closePageFrames[i];
            rightPageTop.sprite = closeCoverFrames[i];
            if(i < 4) rightPageBottomAfterImage.sprite = closeBookFrames[i];
            else rightPageBottomAfterImage.sprite = rightPage;
        }
    }
    
    protected IEnumerator RunCredits(float waitTime)
    {
        while (leftPageBefore.childCount > 0)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            yield return StartCoroutine(PageTurnAnim(0.1f));

            leftPageAfter.GetChild(leftPageAfter.childCount - 1).gameObject.SetActive(false);
            leftPageBefore.GetChild(0).parent = leftPageAfter;
            
            if(leftPageBefore.childCount !=0) leftPageBefore.GetChild(0).gameObject.SetActive(true);
        }
        
        yield return new WaitForSecondsRealtime(waitTime);
    }
    
    IEnumerator PageTurnAnim(float waitTime)
    {
        Image rightPageBottomAfterImage = rightPageBottomAfter.GetComponent<Image>();
        // int i = 0;
        for(int i = 0; i < pageBottomFrames.Length; i++)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            rightPageBottomAfterImage.sprite = pageBottomFrames[i];
            rightPageTop.sprite = pageTopFrames[i];
        }

        rightPageBottomAfter.transform.GetChild(rightPageBottomAfter.transform.childCount - 1).gameObject.SetActive(false);
        rightPageBottomBefore.GetChild(0).parent = rightPageBottomAfter.transform;

        if(rightPageBottomBefore.childCount == 0) rightPageBottomBefore.gameObject.SetActive(false);
        else rightPageBottomBefore.GetChild(0).gameObject.SetActive(true);

        rightPageBottomAfterImage.sprite = pageBottomFrames[0];
        rightPageTop.sprite = pageTopFrames[0];
    }
}
