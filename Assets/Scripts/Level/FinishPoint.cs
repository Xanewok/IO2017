using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YAGTSS.Level
{
    public class FinishPoint : MonoBehaviour
    {
        public string loadSceneName = "MapGenerator";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") == false)
                return;

            SceneManager.LoadScene(loadSceneName);
        }
    }
}
