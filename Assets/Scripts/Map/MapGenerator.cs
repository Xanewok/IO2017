using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public GameObject tile;
    public Vector3 position;
    public SimpleNavMeshGenerator navMeshGenerator;
    public bool buildNavMeshOnGenerate = true;

    private const string kGeneratedSuffix = "(generated)";
    private GameObject[] mapTiles;

    void UpdateTileCache()
    {
        mapTiles = GameObject.FindGameObjectsWithTag("MapTile");
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
        // TODO: Actually sensibly generate map from tiles
        GameObject obj = Instantiate(tile, position, Quaternion.identity);
        obj.name += kGeneratedSuffix;

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
