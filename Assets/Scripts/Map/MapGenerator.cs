using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tile;
    public Vector3 position;

    public const string GENERATED_SUFFIX = "(generated)";

    public void Generate()
    {
        GameObject obj = Instantiate(tile, position, Quaternion.identity);
        obj.name += GENERATED_SUFFIX;
    }

    public void Clear()
    {
        GameObject[] mapTiles = GameObject.FindGameObjectsWithTag("MapTile");
        foreach (GameObject tile in mapTiles)
        {
            if (tile.name.EndsWith(GENERATED_SUFFIX))
            {
                DestroyImmediate(tile);
            }
        }
    }
}
