using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileConnector : MonoBehaviour
{
    public enum State
    {
        Open,
        Connected,
        Rejected,
    }

    [SerializeField]
    State m_state = State.Open;
    public State state { get { return m_state; } set { m_state = value; } }

#if UNITY_EDITOR
    Bounds cachedBounds;

    void Update()
    {
        if (transform.hasChanged)
            cachedBounds = GetTilePhysicalBounds();
    }
#endif

    public Bounds GetTilePhysicalBounds(float margin = 0.0f)
    {
        var result = new Bounds();
        result.center = transform.position;

        var colliders = transform.root.GetComponentsInChildren<Collider>();
        // Sometimes there can be no physical collider at transform.position,
        // yet we want minimal physical bounding box
        if (colliders.Length > 0)
            result = colliders[0].bounds;

        foreach (var collider in colliders)
        {
            result.Encapsulate(collider.bounds);            
        }

        result.Expand(margin);
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;

        Gizmos.color = Color.blue; // local Z-axis
        Gizmos.DrawLine(origin, origin + transform.forward);

        Gizmos.color = Color.red; // local X-axis
        Gizmos.DrawLine(origin, origin + transform.right);

        Gizmos.color = Color.green; // local Y-axis
        Gizmos.DrawLine(origin, origin + transform.up);

        Gizmos.color = GetConnectorStateColor(state);
        Gizmos.DrawWireSphere(origin, 1.0f);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(cachedBounds.center, cachedBounds.size);
    }

    public static Color GetConnectorStateColor(State state)
    {
        switch (state)
        {
            case State.Connected: return Color.green;
            case State.Rejected: return Color.red;
            case State.Open:
            default: return Color.grey;
        }
    }
#endif
}
