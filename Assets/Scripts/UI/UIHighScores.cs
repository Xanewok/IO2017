using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using YAGTSS.Serialization;

public class UIHighScores : MonoBehaviour
{
    [Tooltip("Number of high score entries to display. Make sure the resulting text will fit in the Text field.")]
    public int displayEntriesCount = 5;
    public Text Players;
    public Text Scores;

    private HighScoresManager highScoreManager;

    void Awake()
    {
        highScoreManager = GameController.Instance.GetComponent<HighScoresManager>();
    }

    void OnEnable()
    {
        highScoreManager.OnHighScoresChanged += OnHighScoresChanged;

        RefreshScores();
    }

    void OnDisable()
    {
        highScoreManager.OnHighScoresChanged -= OnHighScoresChanged;
    }

    public void RefreshScores()
    {
        DisplayScore(highScoreManager.highScores);
    }

    public void DisplayScore(HighScores highScores)
    {
        Players.text = "Player";
        Scores.text = "Score";

        var entries = highScores.Entries.Take(displayEntriesCount);
        foreach (var entry in entries)
        {
            Players.text += "\n" + entry.Value;
            Scores.text += "\n" + entry.Key;
        }

        // Fill remaining empty entries with empty '-' markers
        for (int entryCount = entries.Count(); entryCount < displayEntriesCount; ++entryCount)
        {
            Players.text += "\n-";
            Scores.text += "\n-";
        }
    }

    private void OnHighScoresChanged(HighScores scores)
    {
        DisplayScore(scores);
    }
}
