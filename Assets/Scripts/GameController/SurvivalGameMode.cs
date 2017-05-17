using System;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalGameMode : BaseGameMode, IScoredGameMode<Int32>
{
    public event EventHandler OnScoreChanged;

    public UIDeadMenu deadMenu;
    public int scorePerEnemy = 10;

    private HashSet<GameObject> enemies = new HashSet<GameObject>();
    private HashSet<GameObject> players = new HashSet<GameObject>();
    private int scoreCount = 0;

    void Awake()
    {
        GameController.Instance.gameMode = this;
    }

    public override string GetName()
    {
        return "Survival";
    }

    public override GameModeType GetModeType()
    {
        return GameModeType.Survival;
    }

    public override void OnEnemySpawned(GameObject enemy)
    {
        enemies.Add(enemy);
        enemy.GetComponent<EnemyStatus>().healthChanged += OnEnemyHealthChanged;
    }

    public override void OnPlayerSpawned(GameObject player)
    {
        players.Add(player);
        player.GetComponent<PlayerStatus>().healthChanged += OnPlayerHealthChanged;
    }

    public int GetCurrentScore()
    {
        return scoreCount;
    }

    void OnEnemyHealthChanged(GameObject enemy, float health)
    {
        if (health <= 0 && enemies.Contains(enemy))
        {
            enemies.Remove(enemy);

            scoreCount += scorePerEnemy;

            if (OnScoreChanged == null)
                return;

            foreach (var player in players)
            {
                var args = new ScoreChangedEventArgs<int>() { player = player, value = scoreCount };
                OnScoreChanged(player, args);
            }            
        }
    }

    void OnPlayerHealthChanged(GameObject player, float health)
    {
        if (health <= 0)
        {
            deadMenu.gameObject.SetActive(true);
        }
    }

    public int GetScore(GameObject player)
    {
        return scoreCount;
    }

    public int GetMaximumScore(GameObject player)
    {
        return Int32.MaxValue;
    }

    public bool IsScoreCapped()
    {
        return false;
    }
}
