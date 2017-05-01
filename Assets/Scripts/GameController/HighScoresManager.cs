using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.Serialization;

public class HighScoresManager : MonoBehaviour
{
    public delegate void HighScoresChanged(HighScores scores);
    public event HighScoresChanged OnHighScoresChanged;

    private HighScores m_highScores = null;
    public HighScores highScores { get { return m_highScores; } }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Clear();
        }
    }
#endif

    public void Add(string player, int score)
    {
        m_highScores.Add(player, score);

        NotifyListeners();
    }

    public void Clear()
    {
        m_highScores.Clear();

        NotifyListeners();
    }

    public void LoadHighScores(SaveData saveData)
    {
        m_highScores = saveData.highScores;
    }

    private void NotifyListeners()
    {
        if (OnHighScoresChanged != null)
        {
            OnHighScoresChanged(m_highScores);
        }
    }
}
