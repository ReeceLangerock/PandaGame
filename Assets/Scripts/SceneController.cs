using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : SingletonMonobehaviour<SceneController>
{

    private bool isFading;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    public IEnumerator Fade(float fadeOutDuration, float fadeInDuration, float waitInBetween)
    {
        isFading = true;
        float finalAlpha = 1;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeOutDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitInBetween);
        finalAlpha = 0;
        fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeInDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }


        isFading = false;
            yield return null;

    }

    public IEnumerator FadeOutAndIn(float fadeOutDuration = .25f, float fadeInDuration = .5f, float waitInBetween = .5f)
    {
        Debug.Log("Fade");
        faderImage.color = new Color(0f, 0f, 0f, 1f);

        if (!isFading)
        {
            yield return StartCoroutine(Fade(fadeOutDuration, fadeInDuration, waitInBetween));
        }
    }
}
