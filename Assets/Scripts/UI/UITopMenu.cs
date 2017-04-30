using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;

public class UITopMenu : MonoBehaviour
{

    /* If you change this value make sure that output text fit well in top menu. */
    private const int TOP_NR = 5;
    public Text Players;
    public Text Scores;

    /* Load current top players.*/
    public void OnEnable()
    {
        List<KeyValuePair<string, int>> entries = SaveLoad.Load().topData.getEntries();
        Players.text = "Player:";
        Scores.text = "Score: ";
        int output = 0;
        foreach (KeyValuePair<string, int> temp in entries)
        {
            Players.text += "\n" + temp.Key;
            Scores.text += "\n" + temp.Value.ToString();
            if (++output == TOP_NR)
                break;
        }
        for (; output != TOP_NR; ++output)
        {
            Players.text += "\n-";
            Scores.text += "\n-";
        }
    }
}
