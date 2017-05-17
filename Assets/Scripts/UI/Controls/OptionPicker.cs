using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YAGTSS.UI
{
    public class OptionPicker<T> : MonoBehaviour
    {
        public Button optionButton;

        protected List<Button> m_generatedButtons = new List<Button>();
        public List<Button> generatedButtons { get { return m_generatedButtons; } }

        protected Dictionary<T, string> m_values;

        public virtual void InitializeWithValues(KeyValuePair<T, string>[] values)
        {
            m_values = values.ToDictionary(x => x.Key, x => x.Value);

            m_generatedButtons.ForEach(button => Destroy(button.gameObject));
            m_generatedButtons.Clear();

            m_generatedButtons = InstantiateOptionButtons(values);
        }

        protected List<Button> InstantiateOptionButtons(KeyValuePair<T, string>[] values)
        {
            return InstantiateOptionButtons(values, optionButton);
        }

        protected List<Button> InstantiateOptionButtons(KeyValuePair<T, string>[] values, Button prefabButton)
        {
            var transform = this.transform as RectTransform;
            var buttonTransform = optionButton.transform as RectTransform;

            var height = transform.sizeDelta.y;
            var buttonHeight = buttonTransform.sizeDelta.y;

            float heightStep = height / values.Length;

            for (int i = 0; i < values.Length; ++i)
            {
                var button = Instantiate(optionButton, transform);
                m_generatedButtons.Add(button);

                var posY = (height / 2) - i * heightStep - buttonHeight * buttonTransform.pivot.y;
                (button.transform as RectTransform).localPosition = new Vector3(0, posY);

                // Create a local variable so it can properly be moved to handler closure
                var valuePair = values[i];
                button.onClick.AddListener(delegate { OnOptionSelected(valuePair); });

                var buttonText = button.GetComponentInChildren<Text>();
                button.name = buttonText.text = valuePair.Value;
            }

            return m_generatedButtons;
        }

        protected virtual void OnOptionSelected(KeyValuePair<T, string> value)
        {
        }
    }

    public class SelectedOptionPicker<T> : OptionPicker<T>
    {
        [Tooltip("If set to true, Default Option Color will be inferred from Camera Preset Button Text color.")]
        public bool inferDefaultFromPrefab = true;
        public Color defaultOptionColor = new Color32 { r = 50, g = 50, b = 50, a = 255 };
        public Color selectedOptionColor = new Color { r = 0.8f, g = 0, b = 0, a = 1 };

        protected virtual void Awake()
        {
            if (inferDefaultFromPrefab)
                defaultOptionColor = optionButton.GetComponentInChildren<Text>().color;
        }

        protected override void OnOptionSelected(KeyValuePair<T, string> value)
        {
            MarkSelectedOption(value.Key);
        }

        public virtual void MarkSelectedOption(T value, bool deselectRest = true, bool selected = true)
        {
            var buttonName = m_values[value];

            foreach (var button in m_generatedButtons)
            {
                var textComponent = button.GetComponentInChildren<Text>();

                if (textComponent.text == buttonName)
                {
                    textComponent.color = selected ? selectedOptionColor : defaultOptionColor;
                }
                else if (deselectRest)
                {
                    textComponent.color = defaultOptionColor;
                }
            }
        }

        public virtual void MarkSelectedOption(Button button, bool deselectRest = true, bool selected = true)
        {
            if (deselectRest)
                generatedButtons.ForEach(btn => btn.GetComponentInChildren<Text>().color = defaultOptionColor);

            button.GetComponentInChildren<Text>().color = selected ? selectedOptionColor : defaultOptionColor;
        }
    }
}