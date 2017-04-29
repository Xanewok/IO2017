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
                // Calculate matching transform to plug-in
                Vector3 targetPosition = openConnector.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(-openConnector.transform.forward, openConnector.transform.up);

                // Pick a fitting tile
                GameObject spawnedTile = null;

                var shuffledTiles = new List<GameObject>(tileSet);
                shuffledTiles.Shuffle();
                foreach (var tile in shuffledTiles)
                {
                    // Pick a matching connector from available
                    var tileConnectors = tile.GetComponentsInChildren<TileConnector>();
                    foreach (var tileConnector in tileConnectors)
                    {
                        Quaternion toRootRot = tile.transform.rotation * Quaternion.Inverse(tileConnector.transform.rotation);
                        Vector3 toRootTrans = tile.transform.position - tileConnector.transform.position;

                        Quaternion worldConnToTarget = targetRotation * Quaternion.Inverse(tileConnector.transform.rotation);

                        Vector3 finalPosition = targetPosition + worldConnToTarget * toRootTrans;
                        Quaternion finalRotation = toRootRot * worldConnToTarget;

                        // Check for free space
                        // TODO: GetTilePhysicalBounds gives us prefab-based AABB - we want quite precise world OBB instead
                        const float boundsMargin = -0.5f;
                        var bounds = tileConnector.tile.GetPhysicalBounds(boundsMargin);
                        bounds.center = finalPosition; // override center, since tile is a prefab
                        var colliders = Physics.OverlapBox(bounds.center, bounds.extents)
                            .Where(col => col.transform.root.GetInstanceID() != openConnector.transform.root.GetInstanceID());
                        if (colliders.Count() > 0)
                        {
                            Debug.LogFormat("Tile {0} rejected for pos {1}", tile.name, openConnector.transform.position);
                            foreach (var collider in colliders)
                            {
                                Debug.LogFormat("Detected collider: {0} ({1})", collider.name, collider.transform.position);
                            }
                            continue;
                        }

                        spawnedTile = SpawnTile(tile, finalPosition, finalRotation);
                        // Since we checked prefab connections, pass actual object connections outside
                        // It's not pretty, but it's faster than instantiating objects for
                        // every possible match and iterating over spawned connectors
                        tileConnectors = spawnedTile.GetComponentsInChildren<TileConnector>();
                        var spawnedTileConnector = tileConnectors
                            .Where(conn => (conn.transform.position - targetPosition).magnitude < 0.1f)
                            .First();

                        openConnector.Connect(spawnedTileConnector);

                        break;
                    }

                    if (spawnedTile != null)
                    {
                        nextOpenConnectors.AddRange(tileConnectors
                            .Where(conn => conn.state == TileConnector.State.Open));
                        break;
                    }
                }

                // No tile fits, mark this connector as rejected
                if (spawnedTile == null)
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
                                    .ToList();
        edgeTiles.Shuffle();
        foreach (var tile in edgeTiles)
        {
            // FIXME: this is reasonable approach, but needs to be prettied up
            const int attemptCount = 5;
            for (int i = 0; i < attemptCount; ++i)
            {
                const float range = 5.0f;
                Vector3 randomPoint = tile.transform.position + UnityEngine.Random.insideUnitSphere * range;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
                {
                    // Spawn finish point
                    SpawnAuxiliaryObject(finishPoint, hit.position, Quaternion.identity);
                    return;
                }
            }
        }
        // TODO: Handle when we couldn't spawn it
    }
}
