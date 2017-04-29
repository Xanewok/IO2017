using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HelperExtensions;
using System.Linq;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    [Header("Generation")]
    // Supporting arbitrary ITileGenerator requires custom editor for interface type and it's more
    // trouble than it's worth for our use (accepting base MonoBehaviour + ITileGenerator is acceptable)
    public CommonTileGenerator tileGenerator;
    public Vector3 origin;
    public bool generateOnPlayStart = true;
    [Header("Navigation")]
    public SimpleNavMeshGenerator navMeshGenerator;
    public bool buildNavMeshOnGenerate = true;

    void Awake()
    {
        if (tileGenerator == null)
        {
            tileGenerator = GetComponent<CommonTileGenerator>();
        }

        if (navMeshGenerator == null)
        {
            navMeshGenerator = GetComponent<SimpleNavMeshGenerator>();
        }

        if (generateOnPlayStart && IsPlaying())
        {
            Generate();
        }
    }

    public void Generate()
    {
        Clear();

        tileGenerator.BuildBeforeNavMesh(origin);

        if (navMeshGenerator != null && buildNavMeshOnGenerate)
        {
            GameObject[] spawnedTiles = tileGenerator.GetSpawnedTiles();
            navMeshGenerator.BuildNavMesh(spawnedTiles);

            tileGenerator.BuildAfterNavMesh(origin);
        }
    }

    public void Clear()
    {
        if (navMeshGenerator != null)
        {
            navMeshGenerator.RemoveData();
        }

        tileGenerator.Clear();
    }

    static bool IsPlaying()
    {
        return Application.isPlaying
#if UNITY_EDITOR
            || UnityEditor.EditorApplication.isPlaying
#endif
        ;
    }
}

namespace HelperExtensions
{
    public static class MyExtensions
    {
        private static System.Random rng = new System.Random();

        public static GameObject[] FindChildrenWithTag(this Transform transform, string tag)
        {
            var result = new List<GameObject>();
            foreach (Transform childTransform in transform)
            {
                if (childTransform.CompareTag(tag))
                {
                    result.Add(childTransform.gameObject);
                    result.AddRange(FindChildrenWithTag(childTransform, tag));
                }
            }
            return result.ToArray();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
