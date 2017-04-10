using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public GameObject tile;
    public Vector3 position;

    private const string kGeneratedSuffix = "(generated)";
    private GameObject[] mapTiles;

    void UpdateTileCache()
    {
        mapTiles = GameObject.FindGameObjectsWithTag("MapTile");
    }

    public void Generate()
    {
        // TODO: Actually sensibly generate map from tiles
        GameObject obj = Instantiate(tile, position, Quaternion.identity);
        obj.name += kGeneratedSuffix;

        UpdateTileCache();

        // TODO: Make this an option or leave it as mandatory step?
        var navMeshGenerator = GetComponent<SimpleNavMeshGenerator>();
        if (navMeshGenerator != null)
        {
            navMeshGenerator.BuildNavMesh(mapTiles);
        }
    }

    public void Clear()
    {
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
