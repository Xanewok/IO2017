using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    [SerializeField]
    TileConnector[] m_connectors;
    public TileConnector[] connectors { get { return m_connectors; } }

    void Awake()
    {
        if (m_connectors == null)
            m_connectors = GetComponentsInChildren<TileConnector>();
    }

    void Reset()
    {
        m_connectors = GetComponentsInChildren<TileConnector>();
    }

    public Bounds GetPhysicalBounds(float margin = 0.0f)
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

        result.Expand(margin);
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

#if UNITY_EDITOR
    Bounds physicalBounds;

    void Update()
    {
        if (transform.hasChanged)
            physicalBounds = GetPhysicalBounds();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(physicalBounds.center, physicalBounds.size);
    }
#endif

}
