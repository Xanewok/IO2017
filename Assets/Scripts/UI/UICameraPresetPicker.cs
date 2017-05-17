using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAGTSS.UI;
using YAGTSS.Utility;

public class UICameraPresetPicker : SelectedOptionPicker<CameraSettings.CameraType>
{
    protected override void Awake()
    {
        base.Awake();

        var values = EnumHelper.GetValueNameMappingPretty<CameraSettings.CameraType>();
        InitializeWithValues(values);

        var activeCameraType = GameController.Instance.gameSettings.cameraSettings.currentType;
        MarkSelectedOption(activeCameraType);
    }

    protected override void OnOptionSelected(KeyValuePair<CameraSettings.CameraType, string> value)
    {
        GameController.Instance.gameSettings.cameraSettings.currentType = value.Key;
        MarkSelectedOption(value.Key);
    }
}
