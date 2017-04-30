using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Status component for player character
/// </summary>
public class PlayerStatus : Status
{
    protected int score = 0;

    public override void onStart()
    {
        base.onStart();
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
}
