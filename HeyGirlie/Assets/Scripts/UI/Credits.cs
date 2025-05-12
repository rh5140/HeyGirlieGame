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

    void Awake(){
        Pause();
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
        Destroy(creditsMenu);
    }
    
    IEnumerator RunCredits(float waitTime)
    {
        while (leftPageBefore.childCount > 0)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            yield return StartCoroutine(PageTurnAnim(0.1f));

            leftPageAfter.GetChild(leftPageAfter.childCount - 1).gameObject.SetActive(false);
            leftPageBefore.GetChild(0).parent = leftPageAfter;
            
            if(leftPageBefore.childCount !=0) leftPageBefore.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    IEnumerator PageTurnAnim(float waitTime)
    {
        Image rightPageBottomAfterImage = rightPageBottomAfter.GetComponent<Image>();
        int i = 0;
        while(i < pageBottomFrames.Length)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            rightPageBottomAfterImage.sprite = pageBottomFrames[i];
            rightPageTop.sprite = pageTopFrames[i++];
        }

        rightPageBottomAfter.transform.GetChild(rightPageBottomAfter.transform.childCount - 1).gameObject.SetActive(false);
        rightPageBottomBefore.GetChild(0).parent = rightPageBottomAfter.transform;

        if(rightPageBottomBefore.childCount == 0) rightPageBottomBefore.gameObject.SetActive(false);
        else rightPageBottomBefore.GetChild(0).gameObject.SetActive(true);

        rightPageBottomAfterImage.sprite = pageBottomFrames[0];
        rightPageTop.sprite = pageTopFrames[0];
    }
}
