using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameManager : ElementOf<Application>
{
    [SerializeField] private PlayerData playerData;

    public GameStateContext GameStateContext { get; private set; }
    private IGameState _menuState, _playState, _winState;
    private Coroutine _sceneLoadingCoroutine;

    private static GameManager _instance;

    public event Action<Transform> OnPrizeClaimed;
    public event Action OnPrizeClaimCompleted;
    public event Action OnSceneLoaded;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        GameStateContext = new GameStateContext(this);
        GameStateContext.OnCurrentStateChanged += HandleCurrentStateChanged;

        _menuState = gameObject.AddComponent<GameMenuSate>();
        _playState = gameObject.AddComponent<GamePlayState>();
        _winState = gameObject.AddComponent<GameWinState>();

        GameStateContext.Transition(_menuState);
    }

    private void Start()
    {
        Master.OnOrbitNotificationSent += HandleOrbitNotification;
    }

    /// <summary>
    /// On menu state loads current level as additive.
    /// </summary>
    /// <param name="gameState"></param>
    private void HandleCurrentStateChanged(IGameState gameState)
    {
        var menuState = gameState as GameMenuSate;
        if (menuState)
        {
            _sceneLoadingCoroutine = StartCoroutine(LoadCurrentLevel());
        }
    }

    /// <summary>
    /// Invokes prize claim actions through orbit notifications.
    /// </summary>
    /// <param name="stringNotification"></param>
    /// <param name="payload"></param>
    private void HandleOrbitNotification(string stringNotification, Object[] payload)
    {
        if (stringNotification == OrbitNotification._OrbitAttackToPrize)
        {
            var target = payload[0] as Transform;
            if (target)
            {
                OnPrizeClaimed?.Invoke(target);
            }
        }
        else if (stringNotification == OrbitNotification._PrizeAnimationCompleted)
        {
            OnPrizeClaimCompleted?.Invoke();
        }
    }

    /// <summary>
    /// Loads current level additively by player data.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadCurrentLevel()
    {
        var currentLevelNumber = playerData.currentLevelNumber;
        yield return SceneManager.LoadSceneAsync("Level" + currentLevelNumber, LoadSceneMode.Additive);
        yield return new WaitForSeconds(1);
        OnSceneLoaded?.Invoke();
    }


    /// <summary>
    ///  Unloads current level scene.
    ///  Increases current level number on the player data but does not pass level cap.
    /// </summary>
    public void LevelUp()
    {
        StartCoroutine(UnloadLevel(playerData.currentLevelNumber));
        playerData.currentLevelNumber = Mathf.Min(playerData.currentLevelNumber + 1, playerData.levelCap);
        if (GameStateContext.CurrentState != _menuState)
            GameStateContext.Transition(_menuState);
    }

    /// <summary>
    ///  unloads level by level index.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    private IEnumerator UnloadLevel(int level)
    {
        yield return SceneManager.UnloadSceneAsync("Level" + level);
    }

    /// <summary>
    /// starts PlayGameCor coroutine. Triggers via button.
    /// </summary>
    public void PlayGame()
    {
        StartCoroutine(PlayGameCor());
    }

    /// <summary>
    /// Waits for lading level coroutine then change game state to play state.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayGameCor()
    {
        if (GameStateContext.CurrentState == _menuState)
        {
            yield return _sceneLoadingCoroutine;
        }

        if (GameStateContext.CurrentState != _playState)
            GameStateContext.Transition(_playState);
    }

    /// <summary>
    /// When the finish line has been reached, method change game state to win state.
    /// </summary>
    public void WinGame()
    {
        if (GameStateContext.CurrentState != _winState)
            GameStateContext.Transition(_winState);
    }
}