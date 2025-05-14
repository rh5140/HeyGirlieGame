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

    [SerializeField] private AudioClip farewellAudio;
    [SerializeField] private GameObject heyGirlieSticker;
    [SerializeField] private CanvasGroup background;
    [SerializeField] private Image black;

    void OnEnable(){
        if(SceneManager.GetActiveScene().name.Equals("SpringFling")){
            GameObject.Find("AudioTrackManager").GetComponent<AudioTrackManager>().ChangeTrack("default");
        } else {
            Pause();
            LockEsc(EscLock.Credits);
            gameObject.GetComponent<ArrowNavigation>().ArrowKeyStart();
        }

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
        
        int index = 0;
        for (int i = 0; i < closeBookFrames.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            leftPageAfterImage.sprite = closePageFrames[i];
            rightPageTop.sprite = closeCoverFrames[i];
            if(i < 4) rightPageBottomAfterImage.sprite = closeBookFrames[i];
            else rightPageBottomAfterImage.sprite = rightPage;
        }

        if(SceneManager.GetActiveScene().name.Equals("SpringFling")) {
            yield return new WaitForSecondsRealtime(2.5f);

            rightPageBottomBefore.gameObject.SetActive(false);
            rightPageBottomAfter.SetActive(false);
            heyGirlieSticker.SetActive(true);

            float time = 0f, lerpTime = 1f;
            Color blackColor = black.color;
        
            while (time < lerpTime)
            {
                float currentAlpha = Mathf.Lerp(1f, 0f, time / lerpTime);
                background.alpha = currentAlpha;

                Color newColor = Color.Lerp(blackColor, Color.black, time / lerpTime);
                black.color = newColor;

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            background.alpha = 0f;
            black.color = Color.black;

            SettingManager.Instance.voices.PlayOneShot(farewellAudio);
            yield return new WaitWhile(() => SettingManager.Instance.voices.isPlaying);

            SceneManager.LoadScene("Main Menu");
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
