using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileConnector[] m_connectors;
    public TileConnector[] connectors { get { return m_connectors; } }

    private int lastBoundsUpdateFrameId = 0;
    private Bounds m_physicalBounds;
    public Bounds physicalBounds {
        get
        {
            if (Time.frameCount != lastBoundsUpdateFrameId)
            {
                m_physicalBounds = CalculatePhysicalBounds();
                lastBoundsUpdateFrameId = Time.frameCount;
            }

            return m_physicalBounds;
        }
    }

    void Awake()
    {
        if (m_connectors == null)
            m_connectors = GetComponentsInChildren<TileConnector>();
    }

    void Reset()
    {
        m_connectors = GetComponentsInChildren<TileConnector>();
    }

    public Bounds CalculatePhysicalBounds()
    {
        Bounds result;

        var colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length <= 0)
        {
            result = new Bounds { center = transform.position };
        }
        else
        {
            result = colliders[0].bounds;
            foreach (var collider in colliders)
            {
                result.Encapsulate(collider.bounds);
            }
        }

        return result;
    }

    public ConnectorTransform GetTransformToMatch(TileConnector myConnector, TileConnector otherConnector)
    {
        Debug.Assert(connectors.Contains(myConnector));

        var matchingTransform = otherConnector.GetMatchingTransform();

        Quaternion toRootRot = transform.rotation * Quaternion.Inverse(myConnector.transform.rotation);
        Vector3 toRootTrans = transform.position - myConnector.transform.position;

        Quaternion worldConnToTarget = matchingTransform.rotation * Quaternion.Inverse(myConnector.transform.rotation);

        return new ConnectorTransform
        {
            position = matchingTransform.position + worldConnToTarget * toRootTrans,
            rotation = toRootRot * worldConnToTarget
        };
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(physicalBounds.center, physicalBounds.size);
    }
}
