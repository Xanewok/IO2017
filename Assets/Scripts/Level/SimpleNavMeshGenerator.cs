using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace YAGTSS.Level
{
    public class SimpleNavMeshGenerator : MonoBehaviour
    {
        // Explicitly stated for custom Editor
        [SerializeField]
        int m_AgentTypeID;
        public int agentTypeID { get { return m_AgentTypeID; } set { m_AgentTypeID = value; } }

        [SerializeField]
        LayerMask m_LayerMask = ~0;
        public LayerMask layerMask { get { return m_LayerMask; } set { m_LayerMask = value; } }

        [SerializeField]
        NavMeshCollectGeometry m_UseGeometry = NavMeshCollectGeometry.PhysicsColliders;
        public NavMeshCollectGeometry useGeometry { get { return m_UseGeometry; } set { m_UseGeometry = value; } }

        [SerializeField]
        int m_DefaultArea;
        public int defaultArea { get { return m_DefaultArea; } set { m_DefaultArea = value; } }

        private NavMeshData navMeshData;
        private NavMeshDataInstance navMeshDataInstance = new NavMeshDataInstance();

        void OnEnable()
        {
            AddData();
        }

        void OnDisable()
        {
            RemoveData();
        }

        public void AddData()
        {
            if (navMeshDataInstance.valid)
                return;

            if (navMeshData != null)
            {
                navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData, transform.position, transform.rotation);
                navMeshDataInstance.owner = this;
            }
        }

        public void RemoveData()
        {
            navMeshDataInstance.Remove();
            navMeshDataInstance = new NavMeshDataInstance();
        }

        public void BuildNavMesh(GameObject[] fromObjects = null)
        {
            RemoveData();

            var sources = new List<NavMeshBuildSource>();
            var markups = new List<NavMeshBuildMarkup>();

            var buildSettings = NavMesh.GetSettingsByID(agentTypeID);
            if (fromObjects == null)
            {
                NavMeshBuilder.CollectSources(null, layerMask, useGeometry, defaultArea, markups, sources);
            }
            else
            {
                foreach (var obj in fromObjects)
                {
                    var localSources = new List<NavMeshBuildSource>();
                    NavMeshBuilder.CollectSources(obj.transform, layerMask, useGeometry, defaultArea, markups, localSources);
                    sources.AddRange(localSources);
                }
            }

            var bounds = CalculateWorldBounds(transform, sources);
            navMeshData = NavMeshBuilder.BuildNavMeshData(buildSettings, sources, bounds, Vector3.zero, Quaternion.identity);

            AddData();
        }

        // Shamelessly copied from NavMeshSurface.cs
        static Vector3 Abs(Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        static Bounds GetWorldBounds(Matrix4x4 mat, Bounds bounds)
        {
            var absAxisX = Abs(mat.MultiplyVector(Vector3.right));
            var absAxisY = Abs(mat.MultiplyVector(Vector3.up));
            var absAxisZ = Abs(mat.MultiplyVector(Vector3.forward));
            var worldPosition = mat.MultiplyPoint(bounds.center);
            var worldSize = absAxisX * bounds.size.x + absAxisY * bounds.size.y + absAxisZ * bounds.size.z;
            return new Bounds(worldPosition, worldSize);
        }

        static Bounds CalculateWorldBounds(Transform transform, List<NavMeshBuildSource> sources)
        {
            // Use the unscaled matrix for the NavMeshSurface
            Matrix4x4 worldToLocal = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            worldToLocal = worldToLocal.inverse;

            var result = new Bounds();
            foreach (var src in sources)
            {
                switch (src.shape)
                {
                    case NavMeshBuildSourceShape.Mesh:
                        {
                            var m = src.sourceObject as Mesh;
                            result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, m.bounds));
                            break;
                        }
                    case NavMeshBuildSourceShape.Terrain:
                        {
                            // Terrain pivot is lower/left corner - shift bounds accordingly
                            var t = src.sourceObject as TerrainData;
                            result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, new Bounds(0.5f * t.size, t.size)));
                            break;
                        }
                    case NavMeshBuildSourceShape.Box:
                    case NavMeshBuildSourceShape.Sphere:
                    case NavMeshBuildSourceShape.Capsule:
                    case NavMeshBuildSourceShape.ModifierBox:
                        result.Encapsulate(GetWorldBounds(worldToLocal * src.transform, new Bounds(Vector3.zero, src.size)));
                        break;
                }
            }
            // Inflate the bounds a bit to avoid clipping co-planar sources
            result.Expand(0.1f);
            return result;
        }
    }
}
