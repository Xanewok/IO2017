using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.UI;
using YAGTSS.Utility;

public class UIGameModePicker : SelectedOptionPicker<GameModeType>
{
    private GameModeType m_selectedGameMode = GameModeType.Story;
    public GameModeType selectedGameMode { get { return m_selectedGameMode; } }

    protected override void Awake()
    {
        base.Awake();

        //var values = EnumHelper.GetValueNameMappingPretty<GameModeType>();
        // For now do not include "Custom" game mode
        KeyValuePair<GameModeType, string>[] values =
        {
            new KeyValuePair<GameModeType, string>(GameModeType.Story, "Story"),
            new KeyValuePair<GameModeType, string>(GameModeType.Survival, "Survival"),
        };
        InitializeWithValues(values);

        MarkSelectedOption(m_selectedGameMode);
    }

    protected override void OnOptionSelected(KeyValuePair<GameModeType, string> value)
    {
        m_selectedGameMode = value.Key;

        MarkSelectedOption(m_selectedGameMode);
    }
}
