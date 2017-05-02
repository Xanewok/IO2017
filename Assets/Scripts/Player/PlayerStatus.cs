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
    protected int score = 0;

    public override void onStart()
    {
        base.onStart();

        this.healthChanged += OnHealthChanged;

        GameController.Instance.gameMode.OnPlayerSpawned(this.gameObject);
    }

    void Update()
    {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.K))
        {
            hurt(1000, 0);
        }
    }

    public int getScore()
    {
        return score;
    }

    public void addScore(int added)
    {
        score += added;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    void OnHealthChanged(GameObject obj, float health)
    {
        Debug.Assert(obj == this.gameObject);

        if (health <= 0)
        {
            // TODO: Separate this, do it in future GameController class
            //SceneManager.LoadScene("Main_Menu");
        }
    }
}
