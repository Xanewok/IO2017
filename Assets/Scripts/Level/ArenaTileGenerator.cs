using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YAGTSS.Utility;
using YAGTSS.Utility.Extensions;
using UnityEngine.AI;

namespace YAGTSS.Level
{
    public class ArenaTileGenerator : CommonTileGenerator
    {
        [System.Serializable]
        public struct DistanceProbabilityFalloff
        {
            [SerializeField] public float upToDistance;
            [SerializeField] public float percentage;
        }

        public GameObject spawnPoint;
        public GameObject[] tileSet;
        [Tooltip("Each has a certain generation probability depending on how far it is from the map origin")]
        public DistanceProbabilityFalloff[] distanceGenerationProbability;
        [Tooltip("Applied if there is no threshold found in Distance Generation Probability")]
        public float defaultProbability = 0.1f;
        [Header("Enemy Spawns")]
        public GameObject enemySpawner;
        public int enemySpawnerCount = 4;
        public float minimumOriginSpawnerDistance = 15;

        private Vector3 arenaOrigin;
#if UNITY_EDITOR
        public bool debugColorDiscontinuedTiles = false;
        public bool debugColorBorderTiles = false;
#endif

        private void Awake()
        {
            distanceGenerationProbability = distanceGenerationProbability.OrderBy(kv => kv.upToDistance).ToArray();
        }

        public override void BuildBeforeNavMesh(Vector3 origin)
        {
            arenaOrigin = origin;

            SpawnAuxiliaryObject(spawnPoint, origin, Quaternion.identity);
            var originTile = SpawnTile(PickRandomTile(), origin, Quaternion.identity);

            var openConnectors = new List<TileConnector>(originTile.GetComponent<Tile>().connectors);
            while (openConnectors.Count > 0)
            {
                var nextOpenConnectors = new List<TileConnector>();
                foreach (var openConnector in openConnectors)
                {
                    // Discard on tile level, not only on connector level
                    if (!ShouldProcessGeneration(origin, openConnector.transform.position))
                    {
                        continue;
                    }

                    Tile spawnedTile = null;

                    var shuffledTiles = new List<Tile>(tileSet.Select(obj => obj.GetComponent<Tile>()).Shuffle());
                    foreach (Tile prefabTile in shuffledTiles)
                    {
                        BareTransform targetTransform = new BareTransform();
                        TileConnector myConnector = null;

                        if (prefabTile.connectors.Any(conn =>
                        {
                            return prefabTile.CanBeConnectedAndSpawned(conn, openConnector, out targetTransform) && (myConnector = conn);
                        }))
                        {
                            spawnedTile = SpawnTile(prefabTile.gameObject, targetTransform.position, targetTransform.rotation).GetComponent<Tile>();

                            int connectorIndex = Array.IndexOf(prefabTile.connectors, myConnector);
                            openConnector.Connect(spawnedTile.connectors[connectorIndex]);

                            if (ShouldProcessGeneration(origin, spawnedTile.transform.position))
                            {
                                nextOpenConnectors.AddRange(spawnedTile.connectors.Where(conn => conn.state == TileConnector.State.Open));
                            }
                            break;
                        }
                    }

                    // No tile fits, mark this connector as rejected
                    if (!spawnedTile)
                    {
                        openConnector.Reject();
                    }
                }
                openConnectors = nextOpenConnectors;
            }

            BuildWalls();

#if UNITY_EDITOR
            DebugColorTiles();
#endif
        }

        public override void BuildAfterNavMesh(Vector3 origin)
        {
            SpawnEnemySpawners();
        }

        // TODO: This is *really* not ideal. We need proper walls with thickness, not
        // just selecting border tiles and multiplying their Y scale.
        void BuildWalls()
        {
            var borderTiles = GetBorderTiles();
            foreach (var obj in borderTiles)
            {
                var scale = obj.transform.localScale;
                scale.y *= 10.0f;
                obj.transform.localScale = scale;
            }
        }

        void SpawnEnemySpawners()
        {
            HashSet<GameObject> eligibleTiles = new HashSet<GameObject>(GetSpawnedTiles());
            eligibleTiles.RemoveWhere(tile => (tile.transform.position - arenaOrigin).magnitude < minimumOriginSpawnerDistance);
            eligibleTiles.ExceptWith(GetBorderTiles());

            Debug.Assert(eligibleTiles.Count > 0, "No eligible tiles found for enemy spawners!");

            int spawnedCount = 0;
            List<GameObject> shuffledTiles = eligibleTiles.ToList();
            for (int spawnerCount = 0; spawnerCount < enemySpawnerCount; ++spawnerCount)
            {
                shuffledTiles.Shuffle();
                foreach (var tileObject in shuffledTiles)
                {
                    const float range = 5.0f;
                    Vector3 randomPoint = tileObject.transform.position + UnityEngine.Random.insideUnitSphere * range;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
                    {
                        SpawnAuxiliaryObject(enemySpawner, hit.position, Quaternion.identity);
                        spawnedCount++;
                        break;
                    }
                }
            }
            Debug.Assert(spawnedCount == enemySpawnerCount, "Could not spawn all enemy spawners!");
        }

        public GameObject[] GetDiscontinuedTiles()
        {
            return GetSpawnedTiles()
                   .Where(
                       tile => tile.GetComponentsInChildren<TileConnector>()
                       .Where(conn => conn.state == TileConnector.State.Open)
                       .Count() > 0)
                   .ToArray();
        }

        public GameObject[] GetBorderTiles()
        {
            var discontinuedTiles = GetDiscontinuedTiles();

            return discontinuedTiles.Where(tile =>
            {
                return tile.GetComponent<Tile>().connectors
                .Where(conn => conn.state == TileConnector.State.Open)
                .Any(conn =>
                {
                    return Physics.OverlapBox(conn.transform.position, Vector3.one * 0.5f)
                           .Where(col => col.transform.root.gameObject != tile.transform.root.gameObject)
                           .Count() == 0;
                });
            })
            .ToArray();
        }

        public GameObject PickRandomTile()
        {
            int index = UnityEngine.Random.Range(0, tileSet.Length);
            return tileSet[index];
        }

        public bool ShouldProcessGeneration(Vector3 origin, Vector3 dest)
        {
            float random = UnityEngine.Random.Range(0.0f, 1.0f);
            return random < CalculateGenerationProbability(origin, dest);
        }

        // TODO: Move to configuration (AnimationCurve or threshold dictionary)
        public virtual float CalculateGenerationProbability(Vector3 origin, Vector3 dest)
        {
            float tileDistance = (dest - origin).magnitude;

            foreach (var kv in distanceGenerationProbability)
            {
                if (tileDistance <= kv.upToDistance)
                    return kv.percentage;
            }

            return defaultProbability;
        }

#if UNITY_EDITOR
        private static Material discontinuedTileMaterial;
        private static Material borderTileMaterial;
        private static Material initializeBorderTileMaterial()
        {
            var defaultMat = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
            var mat = new Material(defaultMat);
            mat.color = Color.green;
            return mat;
        }

        private void DebugColorTiles()
        {
            if (debugColorDiscontinuedTiles)
            {
                discontinuedTileMaterial = initializeBorderTileMaterial();
                discontinuedTileMaterial.color = discontinuedTileMaterial.color * 3 / 4;

                foreach (var disc in GetDiscontinuedTiles().Select(tile => tile.GetComponentInChildren<MeshRenderer>()))
                    disc.sharedMaterial = discontinuedTileMaterial;
            }

            if (debugColorBorderTiles)
            {
                borderTileMaterial = initializeBorderTileMaterial();

                foreach (var bord in GetBorderTiles().Select(tile => tile.GetComponentInChildren<MeshRenderer>()))
                    bord.sharedMaterial = borderTileMaterial;
            }
        }
#endif
    }
}
