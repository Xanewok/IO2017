using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisableObjectsOnExit : StateMachineBehaviour
{
    public string[] objectNames;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var transforms = animator.gameObject.GetComponentsInChildren<Transform>(true)
            .Where(transform => objectNames.Contains(transform.name));

        foreach (var transform in transforms)
            transform.gameObject.SetActive(false);
    }
}
