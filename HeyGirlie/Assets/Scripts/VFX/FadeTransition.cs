using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    private CanvasGroup ui;
    public float fadeTime = 1f;

    public Image kristenSprite;
    public Image leftSprite;
    public Image rightSprite;
    public Image background; // in case we want to... do something with that

    public GameObject eventSystem;

    public void Start()
    {
        ui = GetComponent<CanvasGroup>();
        ui.alpha = 0f;
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(ui, ui.alpha, 1f, fadeTime));
    }

    public void FadeOut()
    {

        StartCoroutine(FadeCanvasGroup(ui, ui.alpha, 0, fadeTime));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        float time = 0;
        
        while (time < lerpTime)
        {
            float currentAlpha = Mathf.Lerp(start, end, time / lerpTime);
            cg.alpha = currentAlpha;
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        cg.alpha = end;

        yield return null;
        gameObject.GetComponent<ArrowNavigation>().EnableEventSystem();
    }

    public void FadeOutAndChangeScene(string sceneName)
    {
        StartCoroutine(FadeOutAndChangeSceneHelper(sceneName, ui, ui.alpha, 0, fadeTime));
    }

    IEnumerator FadeOutAndChangeSceneHelper(string sceneName,CanvasGroup cg, float start, float end, float lerpTime)
    {
        float time = 0;
        while (time < lerpTime)
        {
            float currentAlpha = Mathf.Lerp(start, end, time / lerpTime);
            cg.alpha = currentAlpha;
            if (kristenSprite.color.a != 0) kristenSprite.color = new Color(kristenSprite.color.r, kristenSprite.color.g, kristenSprite.color.b, currentAlpha);
            if (leftSprite.color.a != 0) leftSprite.color = new Color(leftSprite.color.r, leftSprite.color.g, leftSprite.color.b, currentAlpha);
            if (rightSprite.color.a != 0) rightSprite.color = new Color(rightSprite.color.r, rightSprite.color.g, rightSprite.color.b, currentAlpha);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        cg.alpha = end;
        SceneManager.LoadScene(sceneName);
    }
}
