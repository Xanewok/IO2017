using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public string initialScene = "Main_Menu";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(initialScene);
    }
}
