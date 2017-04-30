using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BareTransform
{
    public Vector3 position;
    public Quaternion rotation;
}

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
    Tile m_tile;
    public Tile tile { get { return m_tile; } }


    [SerializeField]
    State m_state = State.Open;
    public State state { get { return m_state; } }

    [SerializeField]
    TileConnector m_connection = null;
    public TileConnector connection { get { return m_connection; } }

    void Awake()
    {
        if (m_tile == null) m_tile = GetComponentInParent<Tile>();
    }

    void Reset()
    {
        m_tile = GetComponentInParent<Tile>();
        m_state = State.Open;

        var conn = m_connection;
        m_connection = null;
        if (conn)
            conn.Reset();
    }

    public void Connect(TileConnector other)
    {
        m_state = other.m_state = State.Connected;
        m_connection = other;
        other.m_connection = this;
    }

    public void Reject()
    {
        m_state = State.Rejected;
    }

    public BareTransform GetMatchingTransform()
    {
        return new BareTransform
        {
            position = transform.position,
            rotation = Quaternion.LookRotation(-transform.forward, transform.up)
        };
    }

    void OnDrawGizmos()
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

    static Color GetConnectorStateColor(State state)
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