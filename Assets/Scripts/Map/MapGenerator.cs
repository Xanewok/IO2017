using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HelperExtensions;
using System.Linq;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [Header("Generation")]
    public string mapTileTag = "MapTile";
    public GameObject startingTile;
    public Vector3 origin;
    public GameObject[] tiles;
    public int iterationCount = 2;
    public bool generateOnPlayStart = true;
    [Header("Navigation")]
    public SimpleNavMeshGenerator navMeshGenerator;
    public bool buildNavMeshOnGenerate = true;

    private const string kGeneratedSuffix = "(generated)";
    private GameObject[] mapTiles;

    void UpdateTileCache()
    {
        mapTiles = GameObject.FindGameObjectsWithTag(mapTileTag);
    }

    void Awake()
    {
        if (navMeshGenerator == null)
        {
            navMeshGenerator = GetComponent<SimpleNavMeshGenerator>();
        }

        if (generateOnPlayStart && IsPlaying())
        {
            Generate();
        }
    }

    public void Generate()
    {
        Clear();

        {
            GameObject startTile = Instantiate(startingTile, origin, startingTile.transform.rotation);
            startTile.name += kGeneratedSuffix;

            var openConnectors = new List<TileConnector>(startTile.GetComponentsInChildren<TileConnector>());
            if (openConnectors.Count == 0)
            {
                Debug.LogWarning("Starting tile doesn't have any MapTileConnector objects!");
            }

            for (int i = 0; i < iterationCount; ++i)
            {
                var nextOpenConnectors = new List<TileConnector>();
                foreach (var openConnector in openConnectors)
                {
                    // Calculate matching transform to plug-in
                    Vector3 targetPosition = openConnector.transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(-openConnector.transform.forward, openConnector.transform.up);

                    // Pick a fitting tile
                    var shuffledTiles = tiles.Randomize();
                    foreach (var tile in shuffledTiles)
                    {
                        GameObject spawnedTile = null;

                        // Pick a matching connector from available
                        var tileConnectors = tile.GetComponentsInChildren<TileConnector>();
                        foreach (var tileConnector in tileConnectors)
                        {
                            // TODO: Support custom starting rotation (currently only works for identity rotation...)
                            // Factor in connector local position
                            Vector3 finalPosition = targetPosition + tileConnector.transform.localPosition;
                            // Ignore local connector rotation for root world rotation
                            Quaternion finalRotation = targetRotation * Quaternion.Inverse(tileConnector.transform.localRotation);

                            // TODO: Check bounds for overlap and if no possible tile
                            // fits physically here, mark it as rejected

                            // We found a match, spawn tile
                            spawnedTile = Instantiate(tile, finalPosition, finalRotation);
                            spawnedTile.name += kGeneratedSuffix;

                            // Since we checked prefab connections, pass actual object connections outside
                            tileConnectors = spawnedTile.GetComponentsInChildren<TileConnector>();
                            // It's not pretty, but it's faster than instantiating objects for
                            // every possible match and iterating over spawned connectors
                            tileConnectors
                                .Where(conn => (conn.transform.position - targetPosition).magnitude < 0.1f)
                                .First()
                                .state = TileConnector.State.Connected;
                            openConnector.state = TileConnector.State.Connected;

                            break;
                        }

                        if (spawnedTile != null)
                        {
                            nextOpenConnectors.AddRange(tileConnectors
                                .Where(conn => conn.state == TileConnector.State.Open));
                            break;
                        }
                    }
                }
                openConnectors = nextOpenConnectors;
            }
        }

        UpdateTileCache();

        if (navMeshGenerator != null && buildNavMeshOnGenerate)
        {
            navMeshGenerator.BuildNavMesh(mapTiles);
        }
    }

    public void Clear()
    {
        if (navMeshGenerator != null)
        {
            navMeshGenerator.RemoveData();
        }

        UpdateTileCache();
        foreach (GameObject tile in mapTiles)
        {
            if (tile.name.EndsWith(kGeneratedSuffix))
            {
                DestroyImmediate(tile);
            }
        }
        UpdateTileCache();
    }

    static bool IsPlaying()
    {
        return Application.isPlaying
#if UNITY_EDITOR
            || UnityEditor.EditorApplication.isPlaying
#endif
        ;
    }
}

namespace HelperExtensions
{
    public static class MyExtensions
    {
        public static GameObject[] FindChildrenWithTag(this Transform transform, string tag)
        {
            var result = new List<GameObject>();
            foreach (Transform childTransform in transform)
            {
                if (childTransform.CompareTag(tag))
                {
                    result.Add(childTransform.gameObject);
                    result.AddRange(FindChildrenWithTag(childTransform, tag));
                }
            }
            return result.ToArray();
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            System.Random rnd = new System.Random();
            return source.OrderBy(item => rnd.Next());
        }
    }
}
