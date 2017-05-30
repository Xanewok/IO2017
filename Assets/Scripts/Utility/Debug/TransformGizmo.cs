using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGizmo : MonoBehaviour
{
    public float alpha = 1.0f;
    public float length = 1.0f;

    public static Color FromColor(Color col, float alpha = 1.0f)
    {
        return new Color { r = col.r, g = col.g, b = col.b, a = alpha };
    }

    void OnDrawGizmos()
    {
        Vector3 origin = transform.position;

        Gizmos.color = FromColor(Color.blue, alpha); // local Z-axis
        Gizmos.DrawLine(origin, origin + transform.forward * length);

        Gizmos.color = FromColor(Color.red, alpha); // local X-axis
        Gizmos.DrawLine(origin, origin + transform.right * length);

        Gizmos.color = FromColor(Color.green, alpha); // local Y-axis
        Gizmos.DrawLine(origin, origin + transform.up * length);

    }
}
