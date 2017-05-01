using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using YAGTSS.Serialization;

public class UITopMenu : MonoBehaviour
{
    [Tooltip("Number of high score entries to display. Make sure the resulting text will fit in the Text field.")]
    public int displayEntriesCount = 5;
    public Text Players;
    public Text Scores;

    void Update()
    {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F1))
        {
            var data = SaveGameSerializer.Load();
            data.highScores.Clear();
            SaveGameSerializer.Save(data);

            PrepareTextFromData(data.highScores);
        }
    }

    // Load current top players.
    public void OnEnable()
    {
        var highScores = SaveGameSerializer.Load().highScores;
        PrepareTextFromData(highScores);
    }

    private void PrepareTextFromData(HighScores highScores)
    {
        Players.text = "Player:";
        Scores.text = "Score: ";

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
}
