using System.Collections;
using UnityEngine;

namespace YAGTSS.Level
{
    public class SpawnPoint : MonoBehaviour
    {
        void Start()
        {
            SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
            if (spawnPoints.Length > 1)
            {
                Debug.LogWarning("There are more than 1 active objects of type SpawnPoint!");
            }

            // Beam camera so we don't have to wait for it to follow
            StartCoroutine(delayedPlayerBeam());
        }

        IEnumerator delayedPlayerBeam()
        {
            yield return new WaitForEndOfFrame();

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Vector3 camPos = transform.position;
            camPos.y = Camera.current.transform.position.y;
            Camera.current.transform.position = camPos;

            Vector3 spawnPos = transform.position;
            spawnPos.y = player.transform.position.y;
            player.transform.position = spawnPos;
        }
    }
}
