using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPauseMenu : MonoBehaviour
{
    public string pauseButton = "Pause";
    public GameObject toggleObject;
    public GameObject firstSelected;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetButtonDown(pauseButton))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        GameController.Instance.PauseGame(isPaused);

        // Toggle container with pause menu items (background, buttons etc.)
        toggleObject.SetActive(isPaused);
        EventSystem.current.SetSelectedGameObject(isPaused ? firstSelected : null);
    }

    public void QuitToMainMenu()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        GameController.Instance.UnpauseGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
#endif
    }
}
