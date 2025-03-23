using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOnStart : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // Assign the BlackFadeImage in the Inspector
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Start()
    {
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0; // Ensure it's fully transparent
    }
}