using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YAGTSS.Level
{
    public class FinishPoint : MonoBehaviour
    {
        public string loadSceneName = "StoryMode";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") == false)
                return;

            var storyMode = GameController.Instance.gameMode as StoryGameMode;
            if (storyMode)
            {
                storyMode.LevelFinished();
            }

            SceneManager.LoadScene(loadSceneName);
        }
    }
}
