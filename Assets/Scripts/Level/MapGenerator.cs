using UnityEngine;

namespace YAGTSS.Level
{
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
}
