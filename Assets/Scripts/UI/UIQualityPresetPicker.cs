using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.UI;

public class UIQualityPresetPicker : SelectedOptionPicker<int>
{
    protected override void Awake()
    {
        base.Awake();

        InitializeWithValues(GetQualityValues());

        MarkSelectedOption(QualitySettings.GetQualityLevel());
    }

    protected override void OnOptionSelected(KeyValuePair<int, string> value)
    {
        QualitySettings.SetQualityLevel(value.Key);

        MarkSelectedOption(value.Key);
    }

    private static KeyValuePair<int, string>[] GetQualityValues()
    {
        return QualitySettings.names.Select((name, i) => new KeyValuePair<int, string>(i, name)).ToArray();
    }
}
