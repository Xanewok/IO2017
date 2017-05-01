using System.Collections;
using UnityEngine;

public class UIDeadMenu : MonoBehaviour
{
    public float fadeDelay = 0.5f;
    public ImageFader backgroundFader;

    void Start()
    {
        StartCoroutine(BackgroundFade());
    }

    IEnumerator BackgroundFade()
    {
        yield return new WaitForSecondsRealtime(fadeDelay);
        backgroundFader.Fade();
    }
}
