using UnityEngine;
using System.Collections;

public class MapTutorial : MonoBehaviour
{
    public RectTransform rect; 
    public GameObject[] location;

    private GameObject prevLocation;
    private float start, end;

    void OnEnable(){
        start = rect.anchoredPosition.y; end = 0f;
        StartCoroutine(Animate(true));
    }

    IEnumerator Animate(bool awake){
        float time = 0, lerpTime = 0.25f;
        
        if(!awake) (start, end) = (end, start);

        while(time < lerpTime){
            rect.anchoredPosition = new Vector2(0, Mathf.Lerp(start, end, time / lerpTime));

            time += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = new Vector2(0, end);

        if(!awake) gameObject.SetActive(false);
    }

    public void ShowLocation(Region? index){
        if(prevLocation != null) {
            prevLocation.transform.GetChild(0).gameObject.SetActive(true);
            prevLocation.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        if(index != null){
            location[(int)index].transform.GetChild(0).gameObject.SetActive(false);
            location[(int)index].transform.GetChild(1).gameObject.SetActive(true);
            
            prevLocation = location[(int)index];
        } else StartCoroutine(Animate(false));
    }
}
