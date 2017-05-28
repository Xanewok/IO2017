using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectObjectOnEnable : MonoBehaviour
{
    public Selectable selectable;

    private void OnEnable()
    {
        StartCoroutine(DoSelect());
    }

    IEnumerator DoSelect()
    {
        // Wait a single frame as sometimes selectable is logically selected,
        // but it doesn't visually refresh (it's done too fast?) at all
        yield return null;

        selectable.Select();
    }
}
