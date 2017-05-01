using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YAGTSS.Serialization;
using System.Linq;

public class HighScoreInput : MonoBehaviour
{
	public UIDeadMenu deadMenu;
    // TODO: Provide externally or do it in a more robust way
    public PlayerStatus playerScore;
    public UITopMenu highScoresView;

    public string submitButton = "Submit";
    public InputField inputField;
	public Button inputSubmitButton;

    void Start()
    {
		if (deadMenu == null)
		{
			deadMenu = GetComponentInParent<UIDeadMenu>();
		}

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
        var currentScore = GetCurrentScore();

        var saveData = SaveGameSerializer.Load();
        saveData.highScores.Add(inputField.text, currentScore);
        SaveGameSerializer.Save(saveData);

        highScoresView.DisplayScore(saveData.highScores);

		inputField.gameObject.SetActive(false);
		inputSubmitButton.gameObject.SetActive(false);

		deadMenu.FadeToMainMenu();
    }

    // TODO: Provide externally or do it in a more robust way
    public int GetCurrentScore()
    {
        return playerScore.getScore();
    }
}
