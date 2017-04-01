using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform myTransform;

    public Transform followTransform;
    [Range(0.0f, 1.0f)]
    public float followSpeed = 0.5f;

    public void SetFollowTarget(GameObject obj)
    {
        followTransform = obj == null ? null : obj.GetComponent<Transform>();
    }

    void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    void Start()
    {
        if (followTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SetFollowTarget(player);
        }
    }

    void LateUpdate()
    {
        if (followTransform == null)
            return;

        Vector3 offset = (followTransform.position - myTransform.position);
        offset.y = 0; // Follow only in XZ

        myTransform.position += (offset * followSpeed * Time.timeScale);
    }
}
