using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Dropdown : Menu
{
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject paper;
    [SerializeField] private GameObject hoverArea;
    [SerializeField] private TMP_Text tabText;

    [SerializeField] private GameObject saveGalleryMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject quitPopup;
    [SerializeField] private EventTrigger eventTrigger;

    public bool pause = false;
    private float start = 2588f;
    private bool isOpen = false;
    
    private bool animationLock = false;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && !animationLock && GameManager.Instance.escLock == EscLock.Dropdown){
            eventTrigger.OnPointerClick(null);

            if(!isOpen) OpenDropdown();
            else CloseDropdown();
        }
    }

    public void HoverArea(){
        StartCoroutine(HoverDropdown(true));
    }

    public void UnhoverArea(){
        StartCoroutine(HoverDropdown(false));
    }

    public void OpenDropdown(){
        isOpen = true;
        StartCoroutine(AnimateDropdown(true));
        
    }

    public void CloseDropdown(){
        isOpen = false;
        StartCoroutine(AnimateDropdown(false));
    }

    IEnumerator HoverDropdown(bool hover){
        float time = 0, lerpTime = 0.25f;
        RectTransform paperRect = paper.GetComponent<RectTransform>(); 
        float pStart = -2220.5f, pEnd = -2588f;
        if(hover){
            // paper.SetActive(true);
            pEnd = -2220.5f; pStart = -2588f;
        }

        while(time < lerpTime){
            paperRect.anchoredPosition = new Vector2(Mathf.Lerp(pStart, pEnd, time / lerpTime), 0);

            time += Time.deltaTime;
            yield return null;
        }

        if(!hover){
            // paper.SetActive(false);
        }
    }

    public void DisableHover(){
        hoverArea.SetActive(!hoverArea.activeSelf);
    }

    IEnumerator AnimateDropdown(bool open){
        gameObject.GetComponent<ArrowNavigation>().DisableEventSystem();
        yield return null;

        float time = 0, lerpTime = 0.25f;
        Image overlayImg = overlay.GetComponent<Image>();
        RectTransform paperRect = paper.GetComponent<RectTransform>(); 
        
        animationLock = true;

        float cEnd = 0f, cStart = 1f, pEnd = -1*start, pStart = -1457.5f;
        if(open){
            hoverArea.SetActive(false);
            overlay.SetActive(true);
            cStart = 0f; cEnd = 1f; pStart = -1*start; pEnd = -1457.5f;
        } else {
            // "OnDestroy"
            Unpause();
            UnlockEsc();
        }

        while(time < lerpTime){
            Color c = overlayImg.color;
            c.a = Mathf.Lerp(cStart, cEnd, time / lerpTime);
            overlayImg.color = c;

            paperRect.anchoredPosition = new Vector2(Mathf.Lerp(pStart, pEnd, time / lerpTime), 0);

            time += Time.deltaTime;
            yield return null;
        }

        if(!open){
            overlay.SetActive(false);
            hoverArea.SetActive(true);
            ChangeTab("Menu");
        } else {
            // "Awake"
            Pause();
            LockEsc(EscLock.Dropdown);
            ChangeTab("Close");
        }

        animationLock = false;

        yield return null;
        gameObject.GetComponent<ArrowNavigation>().EnableEventSystem();
        if(open) gameObject.GetComponent<ArrowNavigation>().ArrowKeyStart();
        else gameObject.GetComponent<ArrowNavigation>().ArrowKeyEnd();
    }

    public void ChangeTab(string value){
        tabText.text = value;
    }

    public void LoadGame()
    {
        CloseDropdown();
        DisableHover();
        CursorManager.Instance.WaitCursor(() => {
            Instantiate(saveGalleryMenu);
            return true;
        });
    }

    public void Settings()
    {
        CloseDropdown();
        DisableHover();
        Instantiate(settingsMenu);
    }

    public void Credits()
    {
        CloseDropdown();
        DisableHover();
        Instantiate(creditsMenu);
    }

    public void ExitPopup(){
        quitPopup.SetActive(true);
    }

    public void QuitGame()
    {
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_WEBGL)
            SceneManager.LoadScene("Main Menu");
        #else
            Application.Quit();
        #endif
    }
}
