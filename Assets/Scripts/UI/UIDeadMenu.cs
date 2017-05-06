using System.Collections;
using UnityEngine;

public class UIDeadMenu : MonoBehaviour
{
    public float fadeDelay = 0.5f;
    public ImageFader backgroundFader;
    public ImageFader toMainMenuFader;

    void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
    }

    void Start()
    {
        StartCoroutine(BackgroundFade());
    }

    IEnumerator BackgroundFade()
    {
        yield return new WaitForSecondsRealtime(fadeDelay);
        backgroundFader.Fade();
    }

    public void FadeToMainMenu()
    {
        StartCoroutine(ToMainMenuFade());
    }

    IEnumerator ToMainMenuFade()
    {
        yield return new WaitForSecondsRealtime(fadeDelay);
        toMainMenuFader.onFadeFinished += FadeToMainMenuFinished;
        toMainMenuFader.Fade();
    }

    void FadeToMainMenuFinished()
    {
        QuitToMainMenu();
    }

    public void QuitToMainMenu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Time.timeScale = 1.0f; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu"); 
#endif
    }
}
