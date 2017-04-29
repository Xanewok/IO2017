using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using HelperExtensions;

public class DungeonTileGenerator : CommonTileGenerator
{
    public GameObject finishPoint;
    public GameObject startingTile;
    public GameObject[] tileSet;
    public int iterationCount = 2;

    public override void BuildBeforeNavMesh(Vector3 origin)
    {
        GameObject startTile = SpawnTile(startingTile, origin, startingTile.transform.rotation);

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
                        const float margin = -0.5f;
                        var bounds = tileConnector.tile.GetPhysicalBounds(margin);
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
                        int connectorIndex = System.Array.IndexOf(tile.connectors, tileConnector);
                        openConnector.Connect(spawnedTile.connectors[connectorIndex]);

                        nextOpenConnectors.AddRange(spawnedTile.connectors.Where(conn => conn.state == TileConnector.State.Open));
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
        PlaceFinishPoint(); // Uses NavMesh to sample placement position
    }

    private void PlaceFinishPoint()
    {
        List<GameObject> edgeTiles = GetSpawnedTiles()
                                    .Where(
                                        tile => tile.GetComponentsInChildren<TileConnector>()
                                        .Where(conn => conn.state == TileConnector.State.Open)
                                        .Count() > 0)
                                    .Shuffle()
                                    .ToList();
        foreach (var tile in edgeTiles)
        {
            const int attemptCount = 5;
            for (int i = 0; i < attemptCount; ++i)
            {
                const float range = 5.0f;
                Vector3 randomPoint = tile.transform.position + UnityEngine.Random.insideUnitSphere * range;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
                {
                    SpawnAuxiliaryObject(finishPoint, hit.position, Quaternion.identity);
                    return;
                }
            }
        }

        Debug.Assert(false, "Could not place Finish Point object when generating map!");
    }
}
