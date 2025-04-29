using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeSettings : MonoBehaviour
{
    private CanvasGroup ui;
    private float fadeTime = 0.5f;

    public void FadeIn()
    {
        
        ui = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(ui, 0f, 1f, fadeTime));
    }

    public void FadeOut()
    {
        ui = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(ui, 1f, 0f, fadeTime));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float time = 0;
        while (time < lerpTime)
        {
            float currentAlpha = Mathf.Lerp(start, end, time / lerpTime);
            cg.alpha = currentAlpha;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        cg.alpha = end;
    }
}
