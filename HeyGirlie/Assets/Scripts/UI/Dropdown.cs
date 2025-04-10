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

    [SerializeField] private GameObject saveProfilesMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject quitPopup;

    public void OpenDropdown(){
        StartCoroutine(AnimateDropdown(true));
    }

    public void CloseDropdown(){
        StartCoroutine(AnimateDropdown(false));
    }

    IEnumerator AnimateDropdown(bool open){
        float time = 0, lerpTime = 0.25f;
        float start = Mathf.Ceil(((gameObject.GetComponent<RectTransform>().rect.width/2) + (925*2))*1.1f);
        Image overlayImg = overlay.GetComponent<Image>();
        RectTransform paperRect = paper.GetComponent<RectTransform>(); 

        float cEnd = 0f, cStart = 1f, pEnd = -1*start, pStart = -1457.5f;
        if(open){
            overlay.SetActive(true);
            paper.SetActive(true);
            cStart = 0f; cEnd = 1f; pStart = -1*start; pEnd = -1457.5f;
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
            paper.SetActive(false);
        }
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
