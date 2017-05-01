using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public Slider healthBar;

    void Reset()
    {
        healthBar = GetComponent<Slider>();
    }

    void OnEnable()
    {
        playerStatus.healthChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        playerStatus.healthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(GameObject obj, float value)
    {
        healthBar.value = value / playerStatus.getMaxHealth();
    }
}
