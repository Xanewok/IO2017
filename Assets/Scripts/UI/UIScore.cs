using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    public GameObject player;

    private IScoredGameMode<Int32> gameMode;
    private Text text;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        text = GetComponent<Text>();
        gameMode = GameController.Instance.gameMode as IScoredGameMode<Int32>;
    }

    void Start()
    {
        UpdateScoreText(gameMode.GetScore(player));
    }

    void OnEnable()
    {
        gameMode.OnScoreChanged += OnScoreChanged;
    }

    void OnDisable()
    {
        gameMode.OnScoreChanged -= OnScoreChanged;
    }

    void OnScoreChanged(object sender, EventArgs args)
    {
        var concreteArgs = args as ScoreChangedEventArgs<Int32>;
        if (concreteArgs.player == player)
        {
            UpdateScoreText(concreteArgs.value);
        }
    }

    void UpdateScoreText(Int32 value)
    {
        text.text = "Score: " + value;
    }
}
