using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainMenuDefault : StateMachineBehaviour
{
    public string[] defaultButtonNames = new string[] { "Play Button", "High Scores Button", "Quit Button" };
    private Transform[] defaultButtons;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        defaultButtons = defaultButtonNames.Select(button =>
            animator.gameObject.GetComponentsInChildren<Transform>(true).First(comp => comp.name.Equals(button))).ToArray();

        foreach (var transform in defaultButtons)
            transform.gameObject.SetActive(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var transform in defaultButtons)
            transform.gameObject.SetActive(false);
    }

}
