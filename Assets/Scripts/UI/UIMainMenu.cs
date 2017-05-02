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
        GameController.Instance.GetComponent<HighScoresManager>().Clear();
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
