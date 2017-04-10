using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConnectorGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;

        Gizmos.color = Color.blue; // local Z-axis
        Gizmos.DrawLine(origin, origin + transform.forward);

        Gizmos.color = Color.red; // local X-axis
        Gizmos.DrawLine(origin, origin + transform.right);

        Gizmos.color = Color.green; // local Y-axis
        Gizmos.DrawLine(origin, origin + transform.up);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(origin, 1.0f);
    }
}
