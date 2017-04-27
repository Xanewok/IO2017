using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTileGenerator : MonoBehaviour, ITileGenerator
{
    void ITileGenerator.BuildAfterNavMesh(Vector3 origin)
    {
        throw new NotImplementedException();
    }

    void ITileGenerator.BuildBeforeNavMesh(Vector3 origin)
    {
        throw new NotImplementedException();
    }

    void ITileGenerator.Clear()
    {
        throw new NotImplementedException();
    }

    GameObject[] ITileGenerator.GetSpawnedTiles()
    {
        throw new NotImplementedException();
    }
}
