using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    [Range(0.0f, 1.0f)]
    public float followSpeed = 0.05f;
    [Range(0.0f, 50.0f)]
    public float distance = 10.0f;
    public Vector3 rotation = Vector3.zero;

    private Transform myTransform;

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

        // Default position camera is directly above (up) the follow target
        Vector3 toFollowTransform = -(Quaternion.Euler(rotation) * followTransform.up) * distance;

        Vector3 targetPosition = followTransform.position - toFollowTransform;
        Quaternion targetRotation = Quaternion.LookRotation(toFollowTransform);

        Vector3 offset = (targetPosition - myTransform.position);
        myTransform.position += (offset * followSpeed * Time.timeScale);
        myTransform.rotation = targetRotation;
    }
}
