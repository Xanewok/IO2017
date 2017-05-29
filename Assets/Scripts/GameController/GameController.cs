using System.Collections.Generic;
using System.Linq;
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

    [System.Serializable]
    public struct GameModeConfig
    {
        public GameModeType gameModeType;
        public GameObject gameMode;
        public string gameModeScene;
    }

    // TODO: Separate config (and possibly use dictionary as intended, not to duplicate storage)
    [SerializeField]
    private GameModeConfig[] m_gameModesConfig;
    public GameModeConfig[] gameModesConfig { get { return m_gameModesConfig; } }

    [SerializeField]
    private string m_mainMenuSceneName = "Main_Menu";
    public string mainMenuSceneName { get { return m_mainMenuSceneName; } }

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
    public BaseGameMode gameMode { get { return m_gameMode; } }

    private Difficulty m_difficulty = Difficulty.Normal;
    public Difficulty difficulty { get { return m_difficulty; } }

    [SerializeField]
    private GameSettings m_gameSettings;
    public GameSettings gameSettings { get { return m_gameSettings; } set { m_gameSettings = value; } }

    private float m_timeScaleBeforePause = 1.0f;
    private bool m_paused = false;
    public bool paused { get { return m_paused; } set { PauseGame(value); } }

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

        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadSaveGameData();

        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Equals(InitialGameControllerScene))
        {
            SceneManager.LoadScene(mainMenuSceneName);
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

    /// <summary>
    /// (Un)pauses the game depending on <paramref name="pause"/> param.
    /// </summary>
    public void PauseGame(bool pause = true)
    {
        m_paused = pause;
        m_timeScaleBeforePause = pause ? Time.timeScale : m_timeScaleBeforePause;
        Time.timeScale = pause ? 0.0f : m_timeScaleBeforePause;
    }

    /// <summary>
    /// Unpauses the game. Same as PauseGame(false).
    /// </summary>
    public void UnpauseGame()
    {
        PauseGame(false);
    }

    /// <summary>
    /// Starts a new game. This loads a new level and also adds an appropriate BaseGameMode component
    /// and initializes it.
    /// </summary>
    public void StartGame(GameController.Difficulty difficulty, GameModeType gameMode)
    {
        m_difficulty = difficulty;

        var config = m_gameModesConfig.First(entry => entry.gameModeType == gameMode);
        SceneManager.LoadScene(config.gameModeScene);
    }

    /// <summary>
    /// Resets game logic (unpauses if necessary) and loads Main Menu scene,
    /// just like when launching game for the first time)
    /// </summary>
    public void GoToMainMenu()
    {
        UnpauseGame();
        SceneManager.LoadScene(m_mainMenuSceneName);
    }

    // Used to add GameMode object when launching game scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(m_mainMenuSceneName))
        {
            SetCurrentGameMode(null);
        }
        else
        {
            var entries = m_gameModesConfig.Where(entry => entry.gameModeScene.Equals(scene.name));
            if (entries.Count() > 0)
            {
                var entry = entries.First();
                SetCurrentGameMode(entry.gameMode);
            }
        }
    }

    /// <summary>
    /// Removes a currently attached game mode if there is any and for given gameMode object
    /// spawns a new instance of it and attaches it as a current game mode.
    /// </summary>
    /// <param name="gameMode">The game mode.</param>
    private void SetCurrentGameMode(GameObject gameMode)
    {
        RemoveCurrentGameMode();

        if (gameMode)
        {
            Debug.Assert(gameMode.GetComponent<BaseGameMode>() != null, "GameMode object doesn't have required BaseGameMode component!");

            var childGameMode = Instantiate(gameMode);
            childGameMode.transform.SetParent(this.transform);

            m_gameMode = childGameMode.GetComponent<BaseGameMode>();
        }
    }

    private void RemoveCurrentGameMode()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BaseGameMode>())
            {
                Destroy(child.gameObject);
            }
        }

        m_gameMode = null;
    }

}
