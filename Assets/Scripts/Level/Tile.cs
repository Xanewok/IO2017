using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using YAGTSS.Utility;

namespace YAGTSS.Level
{
    [ExecuteInEditMode]
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        private TileConnector[] m_connectors;
        public TileConnector[] connectors { get { return m_connectors; } }

        private int lastBoundsUpdateFrameId = 0;
        private Bounds m_physicalBounds;
        public Bounds physicalBounds
        {
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

        public BareTransform GetTransformToMatch(TileConnector mine, TileConnector target)
        {
            Debug.Assert(connectors.Contains(mine));

            // Calculate rotation of parent tile around child connector to match other connector's rotation
            var matchingRotation = Quaternion.LookRotation(-target.transform.forward, target.transform.up);
            Vector3 connectorToTileOffset = this.transform.position - mine.transform.position;

            Quaternion matchRotate = matchingRotation * Quaternion.Inverse(mine.transform.rotation);
            Vector3 rotatedRootWorldOffset = matchRotate * connectorToTileOffset;

            // Use local rotated tile offset from connector to calculate tile world position from target connector
            return new BareTransform
            {
                position = target.transform.position + rotatedRootWorldOffset,
                rotation = matchRotate * this.transform.rotation,
            };
        }

        public bool CanBeConnectedAndSpawned(TileConnector myConnector, TileConnector otherConnector, out BareTransform targetTransform)
        {
            Debug.Assert(connectors.Contains(myConnector));

            var bounds = this.physicalBounds;
            // Allow for marginal overlap
            const float minExtent = 0.5f;
            bounds.extents = new Vector3
            {
                x = Mathf.Max(minExtent, bounds.extents.x - 0.5f),
                y = Mathf.Max(minExtent, bounds.extents.y - 0.5f),
                z = Mathf.Max(minExtent, bounds.extents.z - 0.5f)
            };

            var matchTransform = GetTransformToMatch(myConnector, otherConnector);
            // We check against rotated AABB - this can further optimized
            // for OBB or split into checks for multiple children colliders
            if (Physics.OverlapBox(matchTransform.position, bounds.extents, matchTransform.rotation)
                .Where(col => col.transform.root.GetInstanceID() != otherConnector.transform.root.GetInstanceID())
                .Count() <= 0)
            {
                targetTransform.position = matchTransform.position;
                targetTransform.rotation = matchTransform.rotation;
                return true;
            }
            else
            {
                targetTransform.position = Vector3.zero;
                targetTransform.rotation = Quaternion.identity;
                return false;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireCube(physicalBounds.center, physicalBounds.size);
        }
    }
}
