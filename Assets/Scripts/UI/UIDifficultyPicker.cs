using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.UI;
using YAGTSS.Utility;

public class UIDifficultyPicker : SelectedOptionPicker<GameController.Difficulty>
{
    private GameController.Difficulty m_difficulty = GameController.Difficulty.Normal;
    public GameController.Difficulty difficulty { get { return m_difficulty; } }

    protected override void Awake()
    {
        base.Awake();
        m_difficulty = GameController.Instance.difficulty;

        var values = EnumHelper.GetValueNameMappingPretty<GameController.Difficulty>();
        InitializeWithValues(values);

        MarkSelectedOption(GameController.Instance.difficulty);
    }

    protected override void OnOptionSelected(KeyValuePair<GameController.Difficulty, string> value)
    {
        m_difficulty = value.Key;
        MarkSelectedOption(m_difficulty);
    }
}
