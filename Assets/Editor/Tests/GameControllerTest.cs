using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections;
using System.Linq;

public class GameControllerTest
{
    public static GameObject CreateGameController()
    {
        var go = new GameObject();
        go.AddComponent<GameController>();
        return go;
    }

    [Test]
    public void PointsToValidInitialScene()
    {
        var gameObject = CreateGameController();
        var gameController = gameObject.GetComponent<GameController>();

        Assert.IsTrue(EditorBuildSettings.scenes.Any(scene =>
        {
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            return scene.enabled && sceneName.Equals(gameController.initialScene);
        }));
    }
}
