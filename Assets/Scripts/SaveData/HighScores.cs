using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YAGTSS.Serialization
{
    [System.Serializable]
    public class HighScores
    {
        // SortedDictionary isn't serializable by default and our collection will be
        // relatively small, so use List<KVPair> with sort after add instead
        private List<KeyValuePair<int, string>> m_entries = new List<KeyValuePair<int, string>>();
        public List<KeyValuePair<int, string>> Entries { get { return m_entries; } }

        public void Add(string player, int score)
        {
            Debug.Assert(score >= 0, "Can't add a negative score to HighScores");
            if (player.Length == 0)
            {
                player = "Anonymous";
            }

            m_entries.Add(new KeyValuePair<int, string>(score, player));
            m_entries.Sort((a, b) => -(a.Key.CompareTo(b.Key)));
        }

        public void Clear()
        {
            m_entries.Clear();
        }
    }
}
