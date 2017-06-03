using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInScreenSpace : MonoBehaviour
{
    public Camera cam;
    public Transform followTransform;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void LateUpdate()
    {
        if (followTransform != null)
        {
            transform.position = cam.WorldToScreenPoint(followTransform.position);
        }
    }
}
