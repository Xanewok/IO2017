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

    // This is needed for on-demand loading of GameController (e.g. when starting
    // an arbitrary scene in PlayMode as opposed to starting build from initial game controller scene.
    [Test]
    public void ResourceLoadable()
    {
        Assert.IsNotNull(Resources.Load<GameObject>("GameController"));
    }

    [Test]
    public void PointsToValidMainMenuScene()
    {
        var gameObject = CreateGameController();
        var gameController = gameObject.GetComponent<GameController>();

        Assert.IsTrue(EditorBuildSettings.scenes.Any(scene =>
        {
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            return scene.enabled && sceneName.Equals(gameController.mainMenuSceneName);
        }));
    }

    [Test]
    public void InitialGameControllerSceneValid()
    {
        Assert.IsTrue(EditorBuildSettings.scenes.Any(scene =>
        {
            var sceneName = Path.GetFileNameWithoutExtension(scene.path);
            return scene.enabled && sceneName.Equals(GameController.InitialGameControllerScene);
        }));
    }

    [Test]
    public void InitialGameControllerSceneFirstInBuildSettings()
    {
        var firstBuildScenePath = EditorBuildSettings.scenes.First().path;
        var firstBuildSceneName = Path.GetFileNameWithoutExtension(firstBuildScenePath);

        Assert.IsTrue(firstBuildSceneName.Equals(GameController.InitialGameControllerScene));
    }
}
