using System.Collections;
using System.Collections.Generic;

// FIXME:
[System.Serializable]
public class TopData
{
    private List<KeyValuePair<string, int>> entries;

    public TopData()
    {
        entries = new List<KeyValuePair<string, int>>();
    }

    public List<KeyValuePair<string, int>> getEntries()
    {
        return new List<KeyValuePair<string, int>>(entries);
    }

    public void addNewScore(string player, int score)
    {
        int i;
        for (i = 0; i != entries.Count; ++i)
        {
            if (entries[i].Value < score)
                break;
        }
        entries.Insert(i, new KeyValuePair<string, int>(player, score));
    }
}
