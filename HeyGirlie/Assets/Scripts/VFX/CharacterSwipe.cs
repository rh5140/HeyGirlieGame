using UnityEngine;
using System;
using System.Collections;

public class CharacterSwipe : MonoBehaviour
{
    [SerializeField] private RectTransform[] characters;
    private bool moveRight = false;
    private float lerpTime = 0.5f;

    public void StartAnimate(int index){
        if(index == 50) return;

        // if(index % 2 == 0) moveRight = false;
        // else moveRight = true;

        StartCoroutine(AnimateStartSwipe(index));
    }

    private IEnumerator AnimateStartSwipe(int i){
        float time = 0f;

        float start = characters[i].anchoredPosition.x;
        float end = 0f;

        while(time < lerpTime){
            characters[i].anchoredPosition = new Vector2(Mathf.Lerp(start, end, time / lerpTime), characters[i].anchoredPosition.y);

            time += Time.deltaTime;
            yield return null;
        }
    }

    public void EndAnimate(int index){
        StartCoroutine(AnimateEndSwipe(index));
    }

    private IEnumerator AnimateEndSwipe(int i){
        float time = 0f;

        float start = characters[i].anchoredPosition.x;
        float end = (i % 2 == 0) ? -2750f : 2750f;

        while(time < lerpTime){
            characters[i].anchoredPosition = new Vector2(Mathf.Lerp(start, end, time / lerpTime), characters[i].anchoredPosition.y);

            time += Time.deltaTime;
            yield return null;
        }
        characters[i].anchoredPosition = new Vector2(end, characters[i].anchoredPosition.y);
        //gameObject.SetActive(false);
    }

}
