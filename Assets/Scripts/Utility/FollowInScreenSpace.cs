using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInScreenSpace : MonoBehaviour
{
    public Camera cam;
    public Transform followTransform;
    public bool remainOnScreen;
    [Range(0.5f, 1.0f)]
    public float screenMargin = 0.9f;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void LateUpdate()
    {
        if (followTransform != null)
        {
            var screenPoint = cam.WorldToScreenPoint(followTransform.position);
            if (remainOnScreen)
            {
                screenPoint.x = Mathf.Clamp(screenPoint.x, (1 - screenMargin) * cam.pixelWidth, screenMargin * cam.pixelWidth);
                screenPoint.y = Mathf.Clamp(screenPoint.y, (1 - screenMargin) * cam.pixelHeight, screenMargin * cam.pixelHeight);
            }
            transform.position = screenPoint;
        }
    }
}
