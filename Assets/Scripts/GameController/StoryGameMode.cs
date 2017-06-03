using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryGameMode : BaseGameMode, IScoredGameMode<Int32>
{
    public event EventHandler OnScoreChanged;

    public UIDeadMenu gameFinishedMenu;
    public int scorePerEnemy = 10;
    public int scorePerLevel = 500;
    public int stageCount = 2;
    private int stagesCompleted = 0;

    private HashSet<GameObject> enemies = new HashSet<GameObject>();
    private HashSet<GameObject> players = new HashSet<GameObject>();
    private int scoreCount = 0;

    private int initialPlayerCount = DefaultPlayerCount;
    public override int GetInitialPlayerCount() { return initialPlayerCount; }
    public override int GetPlayerCount() { return players.Count; }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Used by GameController to initialize GameMode for the first time
    /// in an actual scene (just outside main menu).
    /// </summary>
    public override void InitializeMode(int initialPlayerCount)
    {
        this.initialPlayerCount = initialPlayerCount;
    }

    void Start()
    {
        Initialize();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    void Initialize()
    {
        gameFinishedMenu = Resources.FindObjectsOfTypeAll<UIDeadMenu>()
        .Where(menu => menu.gameObject.scene.isLoaded)
        .First();
    }

    public override string GetName()
    {
        return "Story";
    }

    public override GameModeType GetModeType()
    {
        return GameModeType.Story;
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

            AddScore(scorePerEnemy);
        }
    }

    void OnPlayerHealthChanged(GameObject player, float health)
    {
        if (health <= 0)
        {
            gameFinishedMenu.gameObject.SetActive(true);
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

    private void AddScore(int added, bool broadcast = true)
    {
        scoreCount += added;

        if (broadcast)
        {
            if (OnScoreChanged == null)
                return;

            foreach (var player in players)
            {
                var args = new ScoreChangedEventArgs<int>() { player = player, value = scoreCount };
                OnScoreChanged(player, args);
            }
        }
    }

    public void LevelFinished()
    {
        AddScore(scorePerLevel);

        if (++stagesCompleted >= stageCount)
        {
            gameFinishedMenu.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("StoryMode");
        }
    }
}
