using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerMarker : MonoBehaviour
{
    public int playerNum;

    public GameObject playerMarker;

    private void Reset()
    {
        if (playerMarker == null)
            playerMarker = transform.FindChild("Player Marker").gameObject;
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
        {
            playerMarker.SetActive(true);
            playerMarker.GetComponent<FollowInScreenSpace>().followTransform = playerControls.First().transform;
        }
    }
}
