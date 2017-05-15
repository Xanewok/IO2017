using UnityEngine;
using UnityEngine.SceneManagement;
using YAGTSS.Serialization;

public class GameController : MonoBehaviour
{
    public enum Difficulty
    {
        Normal,
        Hard
    }

    public const string InitialGameControllerScene = "_GameControllerScene";

    private static GameController s_instance = null;
    public static GameController Instance
    {
        get
        {
#if UNITY_EDITOR
            if (s_instance == null)
            {
                Debug.Log("Manually spawning GameController since it doesn't currently exist in the scene" +
                    " (Are you launching game in Play Mode on a separate scene?)");

                var obj = Resources.Load<GameObject>("GameController");
                Instantiate(obj, Vector3.zero, Quaternion.identity);
            }
#endif

            return s_instance;
        }
        private set
        {
            s_instance = value;
        }
    }

    private BaseGameMode m_gameMode = null;
    public BaseGameMode gameMode
    {
        get
        {
            var currentScene = SceneManager.GetActiveScene().name;
            return currentScene.Equals("Main_Menu") ? null : m_gameMode;
        }
        set
        {
            m_gameMode = value;
        }
    }

    private Difficulty m_difficulty = Difficulty.Normal;
    public Difficulty difficulty { get { return m_difficulty; } set { m_difficulty = value; } }

    public string nextLoadedScene = "Main_Menu";

    void Awake()
    {
        // Enforce GameController Singleton guarantee
        if (s_instance != null)
        {
            Debug.LogError("Trying to instantiate another GameController!");
            DestroyImmediate(this.gameObject);
            return;
        }
        s_instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSaveGameData();

        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Equals(InitialGameControllerScene))
        {
            SceneManager.LoadScene(nextLoadedScene);
        }
    }

    public void LoadSaveGameData()
    {
        var saveData = SaveGameSerializer.Load();

        var highScoresManager = GetComponent<HighScoresManager>();
        if (highScoresManager != null)
        {
            highScoresManager.LoadHighScores(saveData);
        }
    }

    public void SaveGameData()
    {
        var saveData = SaveGameSerializer.Load();

        var highScoresManager = GetComponent<HighScoresManager>();
        if (highScoresManager != null)
        {
            saveData.highScores = highScoresManager.highScores;
        }

        SaveGameSerializer.Save(saveData);
    }
}
