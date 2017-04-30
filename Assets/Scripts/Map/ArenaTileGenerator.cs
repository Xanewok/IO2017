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

#if UNITY_EDITOR
    public bool debugColorDiscontinuedTiles = false;
    public bool debugColorBorderTiles = false;
#endif

    public override void BuildBeforeNavMesh(Vector3 origin)
    {
        SpawnAuxiliaryObject(spawnPoint, origin, Quaternion.identity);
        var originTile = SpawnTile(PickRandomTile(), origin, Quaternion.identity);

        var openConnectors = new List<TileConnector>(originTile.GetComponent<Tile>().connectors);
        while (openConnectors.Count > 0)
        {
            var nextOpenConnectors = new List<TileConnector>();
            foreach (var openConnector in openConnectors)
            {
                // Discard on tile level, not only on connector level
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

                    if (prefabTile.connectors.Any(conn =>
                    {
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

        BuildWalls();

#if UNITY_EDITOR
        DebugColorTiles();
#endif
    }

    public override void BuildAfterNavMesh(Vector3 origin)
    {

    }

    // TODO: This is *really* not ideal. We need proper walls with thickness, not
    // just selecting border tiles and multiplying their Y scale.
    void BuildWalls()
    {
        var borderTiles = GetBorderTiles();
        foreach (var obj in borderTiles)
        {
            var scale = obj.transform.localScale;
            scale.y *= 10.0f;
            obj.transform.localScale = scale;
        }
    }

    public GameObject[] GetDiscontinuedTiles()
    {
        return GetSpawnedTiles()
               .Where(
                   tile => tile.GetComponentsInChildren<TileConnector>()
                   .Where(conn => conn.state == TileConnector.State.Open)
                   .Count() > 0)
               .ToArray();
    }

    public GameObject[] GetBorderTiles()
    {
        var discontinuedTiles = GetDiscontinuedTiles();

        return discontinuedTiles.Where(tile =>
        {
            return tile.GetComponent<Tile>().connectors
            .Where(conn => conn.state == TileConnector.State.Open)
            .Any(conn =>
            {
                return Physics.OverlapBox(conn.transform.position, Vector3.one * 0.5f)
                       .Where(col => col.transform.root.gameObject != tile.transform.root.gameObject)
                       .Count() == 0;
            });
        })
        .ToArray();
    }

    public GameObject PickRandomTile()
    {
        int index = UnityEngine.Random.Range(0, tileSet.Length);
        return tileSet[index];
    }

    public bool ShouldProcessGeneration(Vector3 origin, Vector3 dest)
    {
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        return random < CalculateGenerationProbability(origin, dest);
    }

    // TODO: Move to configuration (AnimationCurve or threshold dictionary)
    public virtual float CalculateGenerationProbability(Vector3 origin, Vector3 dest)
    {
        float tileDistance = (dest - origin).magnitude;

        if (tileDistance < 40)
            return 1.0f;
        else if (tileDistance < 60)
            return 0.8f;
        else
            return 0.1f;
    }

#if UNITY_EDITOR
    private static Material discontinuedTileMaterial;
    private static Material borderTileMaterial;
    private static Material initializeBorderTileMaterial()
    {
        var defaultMat = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
        var mat = new Material(defaultMat);
        mat.color = Color.green;
        return mat;
    }

    private void DebugColorTiles()
    {
        if (debugColorDiscontinuedTiles)
        {
            discontinuedTileMaterial = initializeBorderTileMaterial();
            discontinuedTileMaterial.color = discontinuedTileMaterial.color * 3 / 4;

            foreach (var disc in GetDiscontinuedTiles().Select(tile => tile.GetComponentInChildren<MeshRenderer>()))
                disc.sharedMaterial = discontinuedTileMaterial;
        }

        if (debugColorBorderTiles)
        {
            borderTileMaterial = initializeBorderTileMaterial();

            foreach (var bord in GetBorderTiles().Select(tile => tile.GetComponentInChildren<MeshRenderer>()))
                bord.sharedMaterial = borderTileMaterial;
        }
    }
#endif
}
