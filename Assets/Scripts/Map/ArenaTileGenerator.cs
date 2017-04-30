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

                // Pick a fitting tile
                Tile spawnedTile = null;

                var shuffledTiles = new List<Tile>(tileSet.Select(obj => obj.GetComponent<Tile>()).Shuffle());
                foreach (Tile tile in shuffledTiles)
                {
                    // Pick a matching connector from available
                    foreach (var tileConnector in tile.connectors)
                    {
                        var tileMatchTransform = tile.GetTransformToMatch(tileConnector, openConnector);

                        // Check for free space
                        var bounds = tile.physicalBounds;
                        bounds.extents = new Vector3(bounds.extents.x - 0.2f, bounds.extents.y, bounds.extents.z - 0.2f);
                        // We check against rotated AABB - this can further optimized
                        // for OBB or split into checks for multiple children colliders
                        if (Physics.OverlapBox(tileMatchTransform.position, bounds.extents, tileMatchTransform.rotation)
                            .Where(col => col.transform.root.GetInstanceID() != openConnector.transform.root.GetInstanceID())
                            .Count() > 0)
                        {
                            continue;
                        }

                        spawnedTile = SpawnTile(tile.gameObject, tileMatchTransform.position, tileMatchTransform.rotation).GetComponent<Tile>();
                        // Since we checked only prefab object connections, connect and queue actual spawned connectors
                        int connectorIndex = Array.IndexOf(tile.connectors, tileConnector);
                        openConnector.Connect(spawnedTile.connectors[connectorIndex]);

                        if (ShouldProcessGeneration(origin, spawnedTile.transform.position))
                        {
                            nextOpenConnectors.AddRange(spawnedTile.connectors.Where(conn => conn.state == TileConnector.State.Open));
                        }
                        break;
                    }

                    if (spawnedTile)
                        break;
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
