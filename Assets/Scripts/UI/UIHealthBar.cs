using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public int playerNum;

    public GameObject healthBar;
    public Slider healthSlider;
    private Status status;

    private void Reset()
    {
        if (healthBar == null)
            healthBar = transform.Find("Health Bar").gameObject;

        if (healthBar && healthSlider == null)
            healthSlider = healthBar.transform.Find("Bar").GetComponent<Slider>();
    }

    void Start()
    {
        // TODO: Change to event-based OnPlayerSpawned instead
        // of waiting an arbitrary amount of time to initialize
        Invoke("ConnectToPlayer", 0.001f);
    }

    public void ListenTo(Status newStatus)
    {
        if (status)
        {
            status.healthChanged -= OnHealthChanged;
        }

        newStatus.healthChanged += OnHealthChanged;
        status = newStatus;
    }

    void OnHealthChanged(GameObject obj, float value)
    {
        healthSlider.value = value / status.getMaxHealth();
    }

    void ConnectToPlayer()
    {
        var playerControls = GameObject.FindGameObjectsWithTag("Player")
            .Select(player => player.GetComponent<PlayerControlls>())
            .Where(controls => controls.getPlayerNum() == playerNum);

        if (playerControls.Count() > 0)
        {
            ListenTo(playerControls.First().GetComponent<Status>());
            healthBar.SetActive(true);
        }
    }
}
