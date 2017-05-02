using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.Serialization;
using System.Linq;

public class HighScoresManager : MonoBehaviour
{
    public delegate void HighScoresChanged(HighScores scores);
    public event HighScoresChanged OnHighScoresChanged;

    private HighScores m_highScores = null;
    public HighScores highScores { get { return m_highScores; } }

    [SerializeField]
    private int m_maxEntries = 5;
    public int maxEntries { get { return m_maxEntries; } }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Clear();
        }
    }
#endif

    public bool Add(string player, int score)
    {
        if (!CanBeAdded(score))
            return false;

        m_highScores.Add(player, score);

        NotifyListeners();
        return true;
    }

    public void Clear()
    {
        m_highScores.Clear();

        NotifyListeners();
    }

    public bool CanBeAdded(int score)
    {
        return m_highScores.Entries.Count < m_maxEntries ||
            m_highScores.Entries.Any(kv => score > kv.Key);
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
