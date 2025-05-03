using UnityEngine;
using System.Collections;

public class SavePopup : MonoBehaviour
{
    [SerializeField] private RectTransform paper;

    private float start = 2414f;
    private float end = 1683.5f;
    private float y = 860f;
    void Awake()
    {
        // StartCoroutine(Appear());
        StartCoroutine(Appear());
    }

    IEnumerator Appear(){
        yield return new WaitForSeconds(1);

        float time = 0, lerpTime = 0.5f;
        
        while(time < lerpTime){
            paper.anchoredPosition = new Vector2(Mathf.Lerp(start, end, time / lerpTime), y);

            time += Time.deltaTime;
            yield return null;
        }
        paper.anchoredPosition = new Vector2(end, y);
        
        StartCoroutine(GoAway());
    }

    IEnumerator GoAway(){
        yield return new WaitForSeconds(1);

        float time = 0, lerpTime = 0.5f;
        
        while(time < lerpTime){
            paper.anchoredPosition = new Vector2(Mathf.Lerp(end, start, time / lerpTime), y);

            time += Time.deltaTime;
            yield return null;
        }
        paper.anchoredPosition = new Vector2(start, y);
        
        CursorManager.Instance.Load(false);

        Destroy(gameObject);
    }
}
