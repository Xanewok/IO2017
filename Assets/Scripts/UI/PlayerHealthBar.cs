using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public int playerNum;
    public Slider healthBar;

    void Start()
    {
        // TODO: Change to event-based OnPlayerSpawned instead
        // of waiting an arbitrary amount of time to initialize
        Invoke("ConnectToPlayer", 0.1f);
    }

    void Reset()
    {
        healthBar = GetComponent<Slider>();
    }

    public void ListenTo(PlayerStatus newStatus)
    {
        playerStatus.healthChanged -= OnHealthChanged;

        newStatus.healthChanged += OnHealthChanged;
        playerStatus = newStatus;
    }

    void OnEnable()
    {
        if (playerStatus)
            playerStatus.healthChanged += OnHealthChanged;
    }

    void OnDisable()
    {
        if (playerStatus)
            playerStatus.healthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(GameObject obj, float value)
    {
        healthBar.value = value / playerStatus.getMaxHealth();
    }

    void ConnectToPlayer()
    {
        var playerControls = GameObject.FindGameObjectsWithTag("Player")
            .Select(player => player.GetComponent<PlayerControlls>())
            .Where(controls => controls.getPlayerNum() == playerNum);

        if (playerControls.Count() > 0)
            playerStatus = playerControls.First().GetComponent<PlayerStatus>();

        if (playerStatus)
        {
            ListenTo(playerStatus);
        }
    }
}
