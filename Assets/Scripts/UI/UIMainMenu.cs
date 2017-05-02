using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public string playScene = "default";

    public void Play()
    {
        SceneManager.LoadScene(playScene);
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
