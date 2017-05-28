using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeadMenu : MonoBehaviour
{
    public float fadeDelay = 0.5f;
    public ImageFader backgroundFader;
    public ImageFader toMainMenuFader;

    void OnEnable()
    {
        GameController.Instance.PauseGame();
    }

    void OnDisable()
    {
        GameController.Instance.UnpauseGame();
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
        GameController.Instance.UnpauseGame();
        SceneManager.LoadScene("Main_Menu"); 
    }
}
