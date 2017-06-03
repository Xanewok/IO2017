using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Status component for player character
/// </summary>
public class PlayerStatus : Status
{
    public override void onStart()
    {
        base.onStart();

        if (GameController.Instance.gameMode)
            GameController.Instance.gameMode.OnPlayerSpawned(this.gameObject);
    }
}
