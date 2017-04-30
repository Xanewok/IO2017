using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HelperExtensions;

public class ArenaTileGenerator : CommonTileGenerator
{
    public GameObject spawnPoint;
    public GameObject[] tileSet;

    public override void BuildBeforeNavMesh(Vector3 origin)
    {
        SpawnAuxiliaryObject(spawnPoint, origin, Quaternion.identity);
        var initial = SpawnTile(PickRandomTile(), origin, Quaternion.identity);

        var openConnectors = new List<TileConnector>(initial.GetComponent<Tile>().connectors);
        while (openConnectors.Count > 0)
        {
            var nextOpenConnectors = new List<TileConnector>();
            foreach (var openConnector in openConnectors)
            {
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

                    if (prefabTile.connectors.Any(conn => {
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

    public bool ShouldProcessGeneration(Vector3 origin, Vector3 dest)
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        return random < CalculateGenerationProbability(origin, dest);
    }

    // TODO: Move to configuration (AnimationCurve or threshold dictionary)
    public float CalculateGenerationProbability(Vector3 origin, Vector3 dest)
    {
        float tileDistance = (dest - origin).magnitude;

        if (tileDistance < 50)
            return 1.0f;
        else
            return 0.3f;
    }
}
