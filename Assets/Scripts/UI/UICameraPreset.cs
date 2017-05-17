using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YAGTSS.Utility.Extensions;

public class UICameraPreset : MonoBehaviour
{
    [Tooltip("If set to true, Default Option Color will be inferred from Camera Preset Button Text color.")]
    public bool inferDefaultFromPrefab = true;
    public Color defaultOptionColor = new Color32 { r = 50, g = 50, b = 50, a = 255 };
    public Color selectedOptionColor = new Color { r = 0.8f, g = 0, b = 0, a = 1 };
    public Button cameraPresetButton;

    private List<Button> generatedButtons = new List<Button>();

    private void Awake()
    {
        if (inferDefaultFromPrefab)
            defaultOptionColor = cameraPresetButton.GetComponentInChildren<Text>().color;

        var cameraTypes = Enum.GetValues(typeof(CameraSettings.CameraType)) as CameraSettings.CameraType[];
        InstantiateCameraPresetButtons(cameraTypes);
        RefreshSelectedOption();
    }

    public void InstantiateCameraPresetButtons(CameraSettings.CameraType[] cameraTypes)
    {
        if (cameraTypes.Length <= 0)
            return;

        var transform = this.transform as RectTransform;
        var buttonTransform = cameraPresetButton.transform as RectTransform;

        var height = transform.sizeDelta.y;
        var buttonHeight = buttonTransform.sizeDelta.y;

        float heightStep = height / cameraTypes.Length;

        for (int i = 0; i < cameraTypes.Length; ++i)
        {
            var button = Instantiate(cameraPresetButton, transform);
            generatedButtons.Add(button);

            var posY = (height / 2) - i * heightStep - buttonHeight * buttonTransform.pivot.y;
            (button.transform as RectTransform).localPosition = new Vector3(0, posY);

            // Create a local variable so it can properly be moved to handler closure
            CameraSettings.CameraType cameraType = cameraTypes[i];
            button.onClick.AddListener(delegate { OnOptionSelected(cameraType); });

            var typeName = Enum.GetName(typeof(CameraSettings.CameraType), cameraType);
            button.name = typeName;
            button.GetComponentInChildren<Text>().text = typeName.SplitCamelCase();
        }
    }

    private void OnOptionSelected(CameraSettings.CameraType cameraType)
    {
        GameController.Instance.gameSettings.cameraSettings.currentType = cameraType;
        RefreshSelectedOption();
    }

    private void MarkSelectedOption(Button button, bool selected = true)
    {
        button.GetComponentInChildren<Text>().color = selected ? selectedOptionColor : defaultOptionColor;
    }

    private void RefreshSelectedOption()
    {
        var currentType = GameController.Instance.gameSettings.cameraSettings.currentType;
        var typeName = Enum.GetName(typeof(CameraSettings.CameraType), currentType);

        generatedButtons.ForEach(button => MarkSelectedOption(button, button.name == typeName));
    }
}
