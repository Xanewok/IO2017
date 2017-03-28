using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public GameObject followingCamera;
    Transform cameraTransform;
    [Range(0.0f, 1.0f)]
    public float followSpeed = 0.5f;

    Transform myTransform;

    public void SetFollowingCamera(GameObject obj)
    {
        
        followingCamera = obj == null ? null : obj;
        cameraTransform = obj == null ? null : obj.GetComponent<Transform>();
    }

    void Awake()
    {
        SetFollowingCamera(followingCamera);
        myTransform = GetComponent<Transform>();
    }

    void Start()
    {
        if (followingCamera == null)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            SetFollowingCamera(camera);
        }
    }

    void LateUpdate()
    {
        if (followingCamera == null)
            return;

        Vector3 offset = (myTransform.position - cameraTransform.position);
        offset.y = 0; // Follow only in XZ

        cameraTransform.position += (offset * followSpeed);
    }
}
