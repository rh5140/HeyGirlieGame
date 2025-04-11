using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Dropdown : MonoBehaviour
{
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject paper;
    [SerializeField] private GameObject hoverArea;

    [SerializeField] private GameObject saveProfilesMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject quitPopup;

    public bool pause = false;

    public void HoverArea(){
        StartCoroutine(HoverDropdown(true));
    }

    public void UnhoverArea(){
        StartCoroutine(HoverDropdown(false));
    }

    public void OpenDropdown(){
        StartCoroutine(AnimateDropdown(true));
    }

    public void CloseDropdown(){
        StartCoroutine(AnimateDropdown(false));
    }

    IEnumerator HoverDropdown(bool hover){
        float time = 0, lerpTime = 0.25f;
        RectTransform paperRect = paper.GetComponent<RectTransform>(); 
        float pStart = -2220.5f, pEnd = -2385f;
        if(hover){
            paper.SetActive(true);
            pEnd = -2220.5f; pStart = -2385f;
        }
        

        while(time < lerpTime){
            paperRect.anchoredPosition = new Vector2(Mathf.Lerp(pStart, pEnd, time / lerpTime), 0);

            time += Time.deltaTime;
            yield return null;
        }

        if(!hover){
            paper.SetActive(false);
        }
    }

    public void DisableHover(){
        hoverArea.SetActive(!hoverArea.activeSelf);
    }

    IEnumerator AnimateDropdown(bool open){
        float time = 0, lerpTime = 0.25f;
        // float start = Mathf.Ceil(((gameObject.GetComponent<RectTransform>().rect.width/2) + (925*2))*1.1f);
        float start = 2220.5f;
        Image overlayImg = overlay.GetComponent<Image>();
        RectTransform paperRect = paper.GetComponent<RectTransform>(); 

        float cEnd = 0f, cStart = 1f, pEnd = -1*start, pStart = -1457.5f;
        if(open){
            overlay.SetActive(true);
            paper.SetActive(true);
            cStart = 0f; cEnd = 1f; pStart = -1*start; pEnd = -1457.5f;
        } else pause = false;

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
            paper.SetActive(false);
        } else pause = true;
    }

    public void LoadGame()
    {
        paper.SetActive(false);
        overlay.SetActive(false);
        Instantiate(saveProfilesMenu);
    }

    public void Settings()
    {
        paper.SetActive(false);
        overlay.SetActive(false);
        Instantiate(settingsMenu);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
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
