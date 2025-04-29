using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeSettings : MonoBehaviour
{
    private CanvasGroup ui;
    public float fadeTime = 1f;

    public void FadeIn()
    {
        
        ui = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(ui, 0f, 1f, fadeTime));
    }

    public void FadeOut()
    {
        ui = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(ui, 1f, 0, fadeTime));
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
