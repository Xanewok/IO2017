using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Implement fully: implement presentation logic part of PlayerInventory
// class; for now just use it as an entry point to connect to players on spawn
public class UIInventory : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int playerNum;

    public GameObject inventoryChild;

    private void Reset()
    {
        if (inventoryChild == null)
            inventoryChild = transform.FindChild("Inventory").gameObject;
    }

    void Start()
    {
        // TODO: Change to event-based OnPlayerSpawned instead
        // of waiting an arbitrary amount of time to initialize
        Invoke("ConnectToPlayer", 0.1f);
    }

    void ConnectToPlayer()
    {
        var playerControls = GameObject.FindGameObjectsWithTag("Player")
            .Select(player => player.GetComponent<PlayerControlls>())
            .Where(controls => controls.getPlayerNum() == playerNum);

        if (playerControls.Count() > 0)
            playerInventory = playerControls.First().GetComponent<PlayerInventory>();

        if (playerInventory)
        {
            BindToPlayer(playerInventory);
        }
    }

    void BindToPlayer(PlayerInventory newPlayerInventory)
    {
        newPlayerInventory.BindToUI(inventoryChild);
        inventoryChild.GetComponent<FollowInScreenSpace>().followTransform = newPlayerInventory.transform;
    }
}
