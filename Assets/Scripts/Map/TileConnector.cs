using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
