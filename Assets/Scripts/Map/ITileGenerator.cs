using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public interface ITileGenerator
{
    void BuildBeforeNavMesh(Vector3 origin);
    void BuildAfterNavMesh(Vector3 origin);
    void Clear();
    GameObject[] GetSpawnedTiles();
}

public abstract class CommonTileGenerator : MonoBehaviour, ITileGenerator
{
    public string tileTag = "MapTile";
    private HashSet<GameObject> spawnedTiles = new HashSet<GameObject>();
    private HashSet<GameObject> spawnedAuxiliaryObjects = new HashSet<GameObject>();

    public abstract void BuildAfterNavMesh(Vector3 origin);
    public abstract void BuildBeforeNavMesh(Vector3 origin);
    public virtual void Clear()
    {
        foreach (GameObject tile in spawnedTiles)
        {
            DestroyImmediate(tile);
        }
        spawnedTiles.Clear();

        foreach (GameObject obj in spawnedAuxiliaryObjects)
        {
            DestroyImmediate(obj);
        }
        spawnedAuxiliaryObjects.Clear();
    }
    public GameObject[] GetSpawnedTiles()
    {
        return spawnedTiles.ToArray();
    }
    public GameObject[] GetSpawnedAuxiliaryObjects()
    {
        return spawnedAuxiliaryObjects.ToArray();
    }

    protected GameObject SpawnTile(GameObject tilePrefab, Vector3 position, Quaternion rotation)
    {
        Debug.Assert(tilePrefab.CompareTag(tileTag));

        GameObject spawnedTile = Instantiate(tilePrefab, position, rotation);
        spawnedTiles.Add(spawnedTile);

        return spawnedTile;
    }

    protected GameObject SpawnAuxiliaryObject(GameObject objectPrefab, Vector3 position, Quaternion rotation)
    {
        Debug.Assert(!objectPrefab.CompareTag(tileTag));

        GameObject spawnedObject = Instantiate(objectPrefab, position, rotation);
        spawnedAuxiliaryObjects.Add(spawnedObject);

        return spawnedObject;
    }
}
