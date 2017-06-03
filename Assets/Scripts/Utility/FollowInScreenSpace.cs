using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInScreenSpace : MonoBehaviour
{
    public new Camera camera;
    public Transform followTransform;

    void Awake()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (followTransform != null)
        {
            transform.position = camera.WorldToScreenPoint(followTransform.position);
        }
    }
}
