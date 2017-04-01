using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMouse : MonoBehaviour
{
    private PlayerControlls playerControls = null;
    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerControls = player != null ? player.GetComponentInChildren<PlayerControlls>() : null;

        if (playerControls)
            UpdateText(playerControls.supportMouse);
    }

    private void OnEnable()
    {
        if (playerControls)
            UpdateText(playerControls.supportMouse);
    }

    public void ToggleMouseSupport()
    {
        playerControls.supportMouse = !playerControls.supportMouse;
        UpdateText(playerControls.supportMouse);
    }

    void UpdateText(bool enabled)
    {
        text.text = "Mouse: " + (enabled ? "On" : "Off");
    }
}
