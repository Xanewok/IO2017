using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaTileGenerator : CommonTileGenerator
{
    public GameObject spawnPoint;
    public GameObject[] tileSet;

    public override void BuildBeforeNavMesh(Vector3 origin)
    {
        throw new NotImplementedException();

        SpawnAuxiliaryObject(spawnPoint, origin, Quaternion.identity);

        var initial = SpawnTile(PickRandomTile(), origin, Quaternion.identity);
        List<GameObject> activeTiles = new List<GameObject> { initial };
        List<GameObject> currentTiles = new List<GameObject> { initial };
        while (activeTiles.Count > 0)
        {
            foreach (var activeTile in activeTiles)
            {
                // Decide if the tile is so far away it's not considered for expansion anymore
                var probability = UnityEngine.Random.Range(0.0f, 1.0f);
                if (probability > CalculateGenerationProbability(origin, activeTile))
                    continue;
                
                var tileConnectors = activeTile.GetComponentsInChildren<TileConnector>();
            }
        }

    }

    public override void BuildAfterNavMesh(Vector3 origin)
    {
        throw new NotImplementedException();
    }

    public GameObject PickRandomTile()
    {
        int index =  UnityEngine.Random.Range(0, tileSet.Length);
        return tileSet[index];
    }

    // TODO: Move to configuration (AnimationCurve or threshold dictionary)
    public float CalculateGenerationProbability(Vector3 origin, GameObject tileObject)
    {
        float tileDistance = (tileObject.transform.position - origin).sqrMagnitude;

        if (tileDistance < 400)
            return 1.0f;
        else if (tileDistance < 1600)
            return 0.9f;
        else if (tileDistance < 6000)
            return 0.7f;
        else if (tileDistance < 12000)
            return 0.4f;
        else
            return 0.0f;
    }
}
