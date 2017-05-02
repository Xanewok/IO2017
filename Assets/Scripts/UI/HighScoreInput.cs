using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreInput : MonoBehaviour
{
    public GameObject player;
    public UIDeadMenu deadMenu;

    public string submitButton = "Submit";
    public InputField inputField;
    public Button inputSubmitButton;

    private HighScoresManager highScoresManager;

    void Awake()
    {
        if (deadMenu == null)
        {
            deadMenu = GetComponentInParent<UIDeadMenu>();
        }

        highScoresManager = GameController.Instance.GetComponent<HighScoresManager>();
        if (!highScoresManager.CanBeAdded(GetCurrentScore()))
        {
            ProceedToMainMenu();
        }
    }

    void Start()
    {
        // There is no onSubmit event which we can subscribe to and
        // onEndEdit can be called when inputField loses focus
        inputField.onEndEdit.AddListener(fieldValue =>
        {
            if (Input.GetButton(submitButton))
            {
                SubmitScore();
            }
        });
        inputSubmitButton.onClick.AddListener(SubmitScore);
    }

    public void SubmitScore()
    {
        highScoresManager.Add(inputField.text, GetCurrentScore());
        // Be sure to commit saved high score to file now
        GameController.Instance.SaveGameData();

        ProceedToMainMenu();
    }

    int GetCurrentScore()
    {
        var gameMode = GameController.Instance.gameMode as IScoredGameMode<Int32>;
        return gameMode.GetScore(player);
    }

    void ProceedToMainMenu()
    {
        inputField.gameObject.SetActive(false);
        inputSubmitButton.gameObject.SetActive(false);

        deadMenu.FadeToMainMenu();
    }
}
