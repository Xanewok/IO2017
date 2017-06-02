using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.UI;
using YAGTSS.Utility;

public class UIPlayerCountPicker : SelectedOptionPicker<int>
{
    private int m_playerCount = BaseGameMode.DefaultPlayerCount;
    public int playerCount { get { return m_playerCount; } }

    public int maxPlayerCount = 2;

    protected override void Awake()
    {
        base.Awake();

        KeyValuePair<int, string>[] values = new KeyValuePair<int, string>[maxPlayerCount];
        for (int i = 1; i <= maxPlayerCount; ++i)
            values[i - 1] = new KeyValuePair<int, string>(i, i.ToString());

        InitializeWithValues(values);

        MarkSelectedOption(m_playerCount);
    }

    protected override void OnOptionSelected(KeyValuePair<int, string> value)
    {
        m_playerCount = value.Key;
        MarkSelectedOption(m_playerCount);
    }
}
