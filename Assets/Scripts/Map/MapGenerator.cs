using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HelperExtensions;
using System.Linq;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public string mapTileTag = "MapTile";
    public string mapTileConnectorTag = "MapTileConnector";
    public GameObject startingTile;
    public Vector3 origin;
    public GameObject[] tiles;
    public int iterationCount = 2;
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
    }

    public void Generate()
    {
        Clear();

        {
            GameObject startTile = Instantiate(startingTile, origin, Quaternion.identity);
            startTile.name += kGeneratedSuffix;

            var openConnectors = new List<GameObject>(startTile.transform.FindChildrenWithTag(mapTileConnectorTag));
            var currentTiles = new List<GameObject> { startTile };

            for (int i = 0; i < iterationCount; ++i)
            {
                var nextOpenConnectors = new List<GameObject>();
                foreach (var connector in openConnectors)
                {
                    // Calculate matching transform to plug-in
                    Vector3 targetPosition = connector.transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(-connector.transform.forward, connector.transform.up);

                    // Pick a fitting tile
                    var shuffledTiles = tiles.Randomize();
                    foreach (var tile in shuffledTiles)
                    {
                        // Pick a matching connector
                        var tileConnectors = tile.transform.FindChildrenWithTag(mapTileConnectorTag);
                        foreach (var tileConnector in tileConnectors)
                        {
                            // World root position is target connector position, factoring in connector local position
                            Vector3 finalPosition = targetPosition + tileConnector.transform.localPosition;
                            // World root rotation is target connector rotation without local connector rotation
                            Quaternion finalRotation = targetRotation * Quaternion.Inverse(tileConnector.transform.localRotation);

                            GameObject spawnedTile = Instantiate(tile, finalPosition, finalRotation);
                            spawnedTile.name += kGeneratedSuffix;

                            // TODO: Discard closed connectors and pass open connectors for next iteration                            
                        }
                    }
                }
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
