using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// canvas of menu scene
/// </summary>
public class MenuCanvas : ElementOf<GameManager>
{
    [SerializeField] private Image background;
    
    private GameObject _menuCanvas;
    
    private void Awake()
    {
        _menuCanvas = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        Master.GameStateContext.OnCurrentStateChanged += HandleStateChange;
        Master.OnSceneLoaded += HandleSceneLoad;
        SetActiveCanvas(Master.GameStateContext.CurrentState);
    }

    private void OnDestroy()
    {
        Master.GameStateContext.OnCurrentStateChanged -= HandleStateChange;
        Master.OnSceneLoaded -= HandleSceneLoad;
    }
    
    /// <summary>
    /// sets background's alpha values to 0.5 after level scene loaded.
    /// </summary>
    private void HandleSceneLoad()
    {
        background.DOFade(.5f, .5f);
    }

/// <summary>
/// handles game state changes.
/// </summary>
/// <param name="gameState"></param>
    private void HandleStateChange(IGameState gameState)
    {
        SetActiveCanvas(gameState);
    }

/// <summary>
/// if current game state equals to menu state, activates menu canvas.
/// </summary>
/// <param name="gameState"></param>
    private void SetActiveCanvas(IGameState gameState)
    {
        var menuState = gameState as GameMenuSate;
        _menuCanvas.SetActive(menuState);
    }
}