using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
    public override void onStart()
    {
        base.onStart();
        healthChanged += OnHealthChanged;

        GameController.Instance.gameMode.OnEnemySpawned(this.gameObject);
    }

    void OnHealthChanged(GameObject obj, float health)
    {
        if (health <= 0.0f)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
