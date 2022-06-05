using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : ElementOf<GameManager>
{
    [SerializeField] private Image background;
    
    private GameObject _gameCanvas;
    
    private void Awake()
    {
        _gameCanvas = transform.GetChild(0).gameObject;
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
    
    private void HandleSceneLoad()
    {
        background.DOFade(.5f, .5f);
    }


    private void HandleStateChange(IGameState gameState)
    {
        SetActiveCanvas(gameState);
    }

    private void SetActiveCanvas(IGameState gameState)
    {
        var menuState = gameState as GameMenuSate;
        _gameCanvas.SetActive(menuState);
    }
}