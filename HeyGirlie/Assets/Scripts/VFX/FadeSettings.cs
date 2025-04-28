using System.Collections;
using UnityEngine;

public class FadeSettings : FadeTransition
{
    public override void Start()
    {
        // this is empty so FadeIn() isn't called on Start
    }

    public override void FadeIn()
    {
        // the difference from base class is line 14
        ui = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(ui, 0f, 1f, fadeTime));
    }

    public override void FadeOut()
    {
        // the difference from base class is line 21
        ui = GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroup(ui, 1f, 0, fadeTime));
    }

    public override IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float time = 0;
        while (time < lerpTime)
        {
            float currentAlpha = Mathf.Lerp(start, end, time / lerpTime);
            cg.alpha = currentAlpha;
            time += Time.fixedDeltaTime;
            // the difference from base class is line 34
            yield return null;
        }

        cg.alpha = end;
    }
}
