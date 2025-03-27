using UnityEngine;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    private CanvasGroup ui;
    public float fadeTime = 1f;

    public void Start()
    {
        ui = GetComponent<CanvasGroup>();
        ui.alpha = 0f;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(ui, 0, 1f, fadeTime));
    }

    public void FadeOut()
    {

        StartCoroutine(FadeCanvasGroup(ui, 1f, 0, fadeTime));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float time = 0;
        
        while (time < lerpTime)
        {
            float currentAlpha = Mathf.Lerp(start, end, time / lerpTime);
            cg.alpha = currentAlpha;
            time += Time.deltaTime;
            yield return null;
        }

        cg.alpha = end;
    }
}
