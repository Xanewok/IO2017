using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public UIGameModePicker gameModePicker;
    public UIDifficultyPicker difficultyPicker;

    public void Play()
    {
        var gameMode = gameModePicker ? gameModePicker.selectedGameMode : GameModeType.Story;
        var difficulty = difficultyPicker ? difficultyPicker.difficulty : GameController.Difficulty.Normal;

        GameController.Instance.StartGame(difficulty, gameMode);
    }

    public void ClearHighScores()
    {
        var highScoresManager = GameController.Instance.GetComponent<HighScoresManager>();
        int entryCount = highScoresManager.highScores.Entries.Count;

        highScoresManager.Clear();
        // Be sure to save cleared data to file right now if changes were made
        if (entryCount > 0)
        {
            GameController.Instance.SaveGameData();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
