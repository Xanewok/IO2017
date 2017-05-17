using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [System.Serializable]
    public struct GameModeLevel
    {
        public GameModeType gameModeType;
        public string gameModeScene;
    }

    public GameModeLevel[] gameModes;
    public UIGameModePicker gameModePicker;
    public UIDifficultyPicker difficultyPicker;

    private Dictionary<GameModeType, string> m_gameModes;

    private void Awake()
    {
        m_gameModes = gameModes.ToDictionary(x => x.gameModeType, x => x.gameModeScene);
    }

    public void Play()
    {
        var gameMode = gameModePicker ? gameModePicker.selectedGameMode : GameModeType.Regular;
        var difficulty = difficultyPicker ? difficultyPicker.difficulty : GameController.Difficulty.Normal;

        Play(difficulty, gameMode);
    }

    public void Play(GameController.Difficulty difficulty, GameModeType gameMode)
    {
        GameController.Instance.difficulty = difficulty;

        SceneManager.LoadScene(m_gameModes[gameMode]);
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
