using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainMenuHighScores : StateMachineBehaviour
{
    public string backButtonObjectName = "Back Button";
    public string clearButtonObjectName = "Clear Button";

    private UIHighScores highScores;
    private Transform backButton;
    private Transform clearButton;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        highScores = animator.gameObject.GetComponentInChildren<UIHighScores>(true);
        highScores.gameObject.SetActive(true);

        backButton = animator.gameObject.GetComponentsInChildren<Transform>(true).Where(trans => trans.name.Equals(backButtonObjectName)).First();
        backButton.gameObject.SetActive(true);

        clearButton = animator.gameObject.GetComponentsInChildren<Transform>(true).Where(trans => trans.name.Equals(clearButtonObjectName)).First();
        clearButton.gameObject.SetActive(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        highScores.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        clearButton.gameObject.SetActive(false);
    }
}
