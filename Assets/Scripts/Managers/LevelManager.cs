using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private float fadeDuration;
    private CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeToTransparent());
    }
    private void Awake()
    {
        instance = this;
    }
    public void RestartLevel()
    {
        StartCoroutine(FadeToBlackInt(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLevelString(string sceneName)
    {
        StartCoroutine(FadeToBlackString(sceneName));
    }


    private IEnumerator FadeToBlackString(string sceneName)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeToTransparent()
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    private IEnumerator FadeToBlackInt(int buildIndex)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        SceneManager.LoadScene(buildIndex);
    }
}
