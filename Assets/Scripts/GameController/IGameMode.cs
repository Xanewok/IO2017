using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType
{
    Story,
    Survival,

    Custom,
}

public interface IGameMode
{
    string GetName();
    GameModeType GetModeType();

    void OnPlayerSpawned(GameObject player);
    void OnEnemySpawned(GameObject enemy);
}


public class ScoreChangedEventArgs<T> : EventArgs
{
    public GameObject player;
    public T value;
}

public interface IScoredGameMode<T>
{
    event EventHandler OnScoreChanged;

    T GetScore(GameObject player);
    T GetMaximumScore(GameObject player);
    bool IsScoreCapped();
}

public abstract class BaseGameMode : MonoBehaviour, IGameMode
{
    public abstract string GetName();
    public virtual GameModeType GetModeType() { return GameModeType.Custom; }
    public abstract void OnEnemySpawned(GameObject enemy);
    public abstract void OnPlayerSpawned(GameObject player);
}
