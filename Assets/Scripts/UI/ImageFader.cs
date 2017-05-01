using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{

    public delegate void FadeFinished();
    public event FadeFinished onFadeFinished;

    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
    public Image image;
    public float fadeTime = 1.0f;
    public bool respectTimeScale = true;
    public FadeType fadeType = FadeType.FadeIn;

    public void Fade()
    {
        Fade(fadeType, fadeTime, respectTimeScale);
    }

    public void Fade(FadeType fadeType, float fadeTime, bool respectTimeScale = true)
    {
        StopAllCoroutines();
        this.fadeTime = fadeTime;
        this.fadeType = fadeType;
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        float fadeStartTime = GetCurrentTime();
        while (GetCurrentTime() - fadeStartTime < fadeTime)
        {
            float progress = (GetCurrentTime() - fadeStartTime) / fadeTime;
            SetAlpha(GetTargetAlpha(progress));
            yield return null;
        }
        SetAlpha(GetTargetAlpha(1.0f));

        // Check for existing subscribers
        if (onFadeFinished != null)
            onFadeFinished();
    }

    float GetTargetAlpha(float progress)
    {
        if (fadeType == FadeType.FadeIn)
            return progress * 1.0f;
        else
            return 1 - (progress * 1.0f);
    }

    void SetAlpha(float value)
    {
        Color color = image.color;
        color.a = value;
        image.color = color;
    }

    float GetCurrentTime()
    {
        return respectTimeScale ? Time.time : Time.unscaledTime;
    }
}
